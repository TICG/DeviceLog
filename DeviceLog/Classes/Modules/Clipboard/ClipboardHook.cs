using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace DeviceLog.Classes.Modules.Clipboard
{
    internal class ClipboardHook : IDisposable
    {
        internal const int WmDrawclipboard = 0x0308;
        internal const int WmChangecbchain = 0x030D;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private readonly System.Windows.Window _window;
        private HwndSource _hWndSource;
        private IntPtr _hWndNextViewer;

        private bool _isViewing;

        internal delegate void ClipboardEvent(string data);

        internal ClipboardEvent ClipboardChanged;

        internal ClipboardHook(System.Windows.Window window)
        {
            _window = window;
        }

        ~ClipboardHook()
        {
            Dispose();
        }

        internal void Hook()
        {
            if (_isViewing) return;

            WindowInteropHelper wih = new WindowInteropHelper(_window);
            _hWndSource = HwndSource.FromHwnd(wih.Handle);

            if (_hWndSource == null) return;

            _hWndSource.AddHook(WinProc);
            _hWndNextViewer = SetClipboardViewer(_hWndSource.Handle);
            _isViewing = true;
        }

        internal void Unhook()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_hWndSource != null)
            {
                ChangeClipboardChain(_hWndSource.Handle, _hWndNextViewer);

                _hWndNextViewer = IntPtr.Zero;
                _hWndSource.RemoveHook(WinProc);
            }
            _isViewing = false;
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WmChangecbchain:
                    if (wParam == _hWndNextViewer)
                    {
                        _hWndNextViewer = lParam;
                    }
                    else if (_hWndNextViewer != IntPtr.Zero)
                    {
                        SendMessage(_hWndNextViewer, msg, wParam, lParam);
                    }
                    break;

                case WmDrawclipboard:
                    if (System.Windows.Clipboard.ContainsText())
                    {
                        ClipboardChanged?.Invoke(System.Windows.Clipboard.GetText());
                    }
                    SendMessage(_hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}
