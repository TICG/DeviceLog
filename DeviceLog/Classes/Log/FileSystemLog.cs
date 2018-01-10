using System;

namespace DeviceLog.Classes.Log
{
    internal class FileSystemLog : Log
    {

        private bool _logDate;

        /// <summary>
        /// Initialize a new FileSystemLog
        /// </summary>
        internal FileSystemLog(bool logDate)
        {
            LogType = LogType.FileSystem;
            Data = "";
            _logDate = logDate;
        }

        internal void SetLogDate(bool log)
        {
            _logDate = log;
        }

        /// <summary>
        /// Add new data to the current log
        /// </summary>
        /// <param name="key">The data that should be added to the log</param>
        internal new void AddData(string key)
        {
            if (Data.Length != 0)
            {
                Data += Environment.NewLine;
            }

            if (_logDate)
            {
                Data += "[" + DateTime.Now + "]";
            }

            Data += key;

            LogUpdatedEvent?.Invoke(Data);
        }
    }
}
