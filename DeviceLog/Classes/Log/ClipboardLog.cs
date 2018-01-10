using System;

namespace DeviceLog.Classes.Log
{
    /// <inheritdoc />
    /// <summary>
    /// A class that represents collected clipboard data
    /// </summary>
    internal class ClipboardLog : Log
    {
        /// <summary>
        /// A boolean to indicate whether or not the date and time should be logged
        /// </summary>
        private bool _logDate;

        /// <summary>
        /// Initialize a new ClipBoardLog object
        /// </summary>
        /// <param name="logDate">A boolean to indicate whether the date and time should be logged</param>
        internal ClipboardLog(bool logDate)
        {
            LogType = LogType.Clipboard;
            Data = "";
            _logDate = logDate;
        }

        /// <summary>
        /// Set whether date and time should be logged
        /// </summary>
        /// <param name="log">A boolean to indicate whether date and time should be logged or not</param>
        internal void SetLogDate(bool log)
        {
            _logDate = log;
        }

        /// <summary>
        /// Add raw string data to the current log
        /// </summary>
        /// <param name="data">The raw string data that should be added to the log</param>
        internal new void AddData(string data)
        {
            if (Data.Length != 0)
            {
                Data += Environment.NewLine;
            }

            if (_logDate)
            {
                Data += "[" + DateTime.Now + "]";
            }
            Data += data;

            LogUpdatedEvent?.Invoke(Data);
        }
    }
}
