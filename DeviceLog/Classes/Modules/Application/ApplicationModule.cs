using DeviceLog.Classes.Log;

namespace DeviceLog.Classes.Modules.Application
{
    internal class ApplicationModule
    {
        private readonly ApplicationLog _applicationLog;

        internal ApplicationModule(bool logDate, LogController logController)
        {
            _applicationLog = new ApplicationLog(logDate);
            logController.AddLog(_applicationLog);
        }

        internal void AddData(string data)
        {
            _applicationLog.AddData(data);
        }

        internal ApplicationLog GetLog()
        {
            return _applicationLog;
        }
    }
}
