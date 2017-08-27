using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceLog.Classes.Log;

namespace DeviceLog.Classes.Modules.Monitor
{
    internal sealed class MonitorModule
    {
        private int _delay;
        private bool _active;

        private MonitorLog _log;

        internal MonitorModule() : this(60000)
        {
        }

        internal MonitorModule(int delay)
        {
            if (delay <= 0) throw new ArgumentException("Monitor delay cannot be equal to or smaller than zero!");

            _delay = delay;
            _active = false;
            _log = new MonitorLog();
        }

        internal void Start()
        {
            
        }

        internal void Stop()
        {
            
        }
    }
}
