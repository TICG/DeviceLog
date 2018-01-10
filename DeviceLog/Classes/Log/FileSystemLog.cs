using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceLog.Classes.Log
{
    internal class FileSystemLog : Log
    {
        /// <summary>
        /// Initialize a new FileSystemLog
        /// </summary>
        internal FileSystemLog()
        {
            LogType = LogType.Keyboard;
            Data = "";
        }

        /// <summary>
        /// Add new data to the current log
        /// </summary>
        /// <param name="key">The data that should be added to the log</param>
        internal new void AddData(string key)
        {
            Data += key;
            LogUpdatedEvent?.Invoke(Data);
        }
    }
}
