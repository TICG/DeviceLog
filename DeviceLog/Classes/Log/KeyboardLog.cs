
using System;
using System.Diagnostics;
using System.Windows;

namespace DeviceLog.Classes.Log
{

    /// <summary>
    /// A keyboard log class. This contains log information for the keyboard module
    /// </summary>
    internal class KeyboardLog : Log
    {
        /// <summary>
        /// The raw string data
        /// </summary>
        private string _data;

        /// <summary>
        /// Initialize a new KeyboardLog
        /// </summary>
        internal KeyboardLog()
        {
            _data = "";
            LogType = LogType.Keyboard;
        }

        /// <summary>
        /// Add new data to the current log
        /// </summary>
        /// <param name="key">The data that should be added to the log</param>
        internal void AddKey(string key)
        {
            _data += key;
        }

        /// <summary>
        /// Get the raw string data that was collected
        /// </summary>
        /// <returns>The raw string data that was previously collected</returns>
        internal override string GetLog()
        {
            return _data;
        }
    }
}
