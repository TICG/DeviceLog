using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace DeviceLog.Classes.Modules.Keyboard
{
    /// <summary>
    ///     A class that manages a global low level keyboard hook
    /// </summary>
    internal class KeyboardHook
    {
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, CallbackDelegate lpfn, int hInstance, int threadId);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, ref KbdllHookStruct lParam);

        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        private delegate int CallbackDelegate(int code, int w, ref KbdllHookStruct l);

        private const byte VkShift = 0x10;
        private const byte VkCapital = 0x14;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct KbdllHookStruct
        {
            internal readonly uint vkCode;
            internal readonly uint scanCode;
            internal readonly uint flags;
            private readonly uint time;
            private readonly uint dwExtraInfo;
        }

        internal enum KeyEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        private int _hookId;

        private bool _isFinalized;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private CallbackDelegate _theHookCb;

        internal delegate void KeyPressedEvent(string key);
        internal KeyPressedEvent KeyUp;
        internal KeyPressedEvent KeyDown;

        private readonly bool _logSpecialKeys;
        private readonly bool _logControlKeys;

        internal KeyboardHook(bool logSpecial, bool logControl)
        {
            _logSpecialKeys = logSpecial;
            _logControlKeys = logControl;
        }

        internal void Hook()
        {
            _theHookCb = KeybHookProc;
            _hookId = SetWindowsHookEx(13, _theHookCb, 0, 0);

            _isFinalized = false;
        }

        internal void Unhook()
        {
            Dispose();
        }

        ~KeyboardHook()
        {
            Dispose();
        }

        internal void Dispose()
        {
            if (_isFinalized) return;
            UnhookWindowsHookEx(_hookId);
            _isFinalized = true;
        }

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

            // Key can be translated to unicode
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
            // Key cannot be translated to unicode
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