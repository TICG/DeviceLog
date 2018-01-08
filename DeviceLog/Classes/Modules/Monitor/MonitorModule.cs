using System;
using DeviceLog.Classes.Log;

namespace DeviceLog.Classes.Modules.Monitor
{
    internal sealed class MonitorModule
    {
        private int _delay;
        private bool _active;

        private MonitorLog _log;

        internal MonitorModule(LogController logController) : this(60000, logController)
        {
        }

        internal MonitorModule(int delay, LogController logController)
        {
            if (delay <= 0) throw new ArgumentException("Monitor delay cannot be equal to or smaller than zero!");

            _delay = delay;
            _active = false;
            _log = new MonitorLog();

            logController.AddLog(_log);
        }

        internal void Start()
        {
            
        }

        internal void Stop()
        {
            
        }
    }
}
