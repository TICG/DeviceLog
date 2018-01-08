using DeviceLog.Classes.Log;

namespace DeviceLog.Classes.Modules.Clipboard
{
    internal sealed class ClipboardModule
    {
        private readonly ClipboardHook _clipboardHook;
        private readonly ClipboardLog _clipboardLog;

        internal ClipboardModule(System.Windows.Window window, bool logDate, LogController logController)
        {
            _clipboardHook = new ClipboardHook(window);
            _clipboardLog = new ClipboardLog(logDate);

            _clipboardHook.ClipboardChanged += ClipboardChanged;

            logController.AddLog(_clipboardLog);
        }

        internal void SetLogDate(bool log)
        {
            _clipboardLog?.SetLogDate(log);
        }

        private void ClipboardChanged(string data)
        {
            _clipboardLog.AddData(data);
        }

        internal void Start()
        {
            _clipboardHook.Hook();
        }

        internal void Stop()
        {
            _clipboardHook.Unhook();
        }

        internal ClipboardLog GetLog()
        {
            return _clipboardLog;
        }
    }
}
