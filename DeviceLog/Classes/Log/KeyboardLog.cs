using System;

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
        internal void AddKey(String key)
        {
            Console.Write(key);
            _data += key;
        }

        /// <summary>
        /// Get the current raw log data
        /// </summary>
        /// <returns>The current raw log data</returns>
        internal string GetLog()
        {
            return _data;
        }
    }
}
