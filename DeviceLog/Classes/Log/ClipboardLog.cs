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
        /// The raw collected data from the clipboard
        /// </summary>
        private string _data;
        /// <summary>
        /// A boolean to indicate whether or not the date and time should be logged
        /// </summary>
        private readonly bool _logDate;

        /// <summary>
        /// Initialize a new ClipBoardLog object
        /// </summary>
        /// <param name="logDate">A boolean to indicate whether the date and time should be logged</param>
        internal ClipboardLog(bool logDate)
        {
            LogType = LogType.Clipboard;
            _data = "";
            _logDate = logDate;
        }

        /// <summary>
        /// Add raw string data to the current log
        /// </summary>
        /// <param name="data">The raw string data that should be added to the log</param>
        internal void AddData(string data)
        {
            _data += Environment.NewLine;
            if (_logDate)
            {
                _data += "[" + DateTime.Now + "]" + data;
            }
            else
            {
                _data += Environment.NewLine;
                _data += data;
            }
        }

        /// <summary>
        /// Get the raw string data that was collected
        /// </summary>
        /// <returns>The raw string data that was previously collected</returns>
        internal string GetLog()
        {
            return _data;
        }
    }
}
