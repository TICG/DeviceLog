using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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

        private const byte VkReturn = 0X0D; //Enter
        private const byte VkSpace = 0X20; //Space
        private const byte VkShift = 0x10;
        private const byte VkCapital = 0x14;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct KbdllHookStruct
        {
            internal uint vkCode;
            internal uint scanCode;
            internal uint flags;
            private uint time;
            private uint dwExtraInfo;
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

        //Start hook
        internal KeyboardHook()
        {
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

        //The listener that will trigger events
        private int KeybHookProc(int code, int w, ref KbdllHookStruct l)
        {
            if (code < 0) return CallNextHookEx(_hookId, code, w, ref l);

            KeyEvents kEvent = (KeyEvents)w;

            byte[] keyState = new byte[256];
            GetKeyboardState(keyState);

            StringBuilder sbString = new StringBuilder(10);

            int res = ToUnicodeEx(l.vkCode, l.scanCode, keyState, sbString, sbString.Capacity, l.flags, GetKeyboardLayout(0));

            if (res == 1)
            {
                char key = sbString[0];
                bool isDownShift = ((GetKeyState(VkShift) & 0x80) == 0x80);
                bool isDownCapslock = (GetKeyState(VkCapital) != 0);
                if ((isDownCapslock ^ isDownShift) && char.IsLetter(key))
                {
                    key = char.ToUpper(key);
                }

                string result = key.ToString();
                
                if (kEvent == KeyEvents.KeyDown || kEvent == KeyEvents.SKeyDown)
                {
                    KeyUp?.Invoke(result);
                }
                else if (kEvent == KeyEvents.KeyUp || kEvent == KeyEvents.SKeyUp)
                {
                    KeyDown?.Invoke(result);
                }
            }
            // Key cannot be translated to unicode
            else if (res == 0)
            {
                Keys key = (Keys)l.vkCode ;
                if (kEvent == KeyEvents.KeyDown || kEvent == KeyEvents.SKeyDown)
                {
                    KeyUp?.Invoke(key.ToString());
                }
                else if (kEvent == KeyEvents.KeyUp || kEvent == KeyEvents.SKeyUp)
                {
                    KeyDown?.Invoke(key.ToString());
                }
            }
            return CallNextHookEx(_hookId, code, w, ref l);
        }
    }
}