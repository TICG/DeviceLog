using System;
using System.Globalization;

namespace DeviceLog.Classes.Log
{
    /// <summary>
    /// A class that represent a change in the application
    /// </summary>
    internal class ApplicationLog : Log
    {
        /// <summary>
        /// The raw log data
        /// </summary>
        private string _data;

        /// <summary>
        /// A boolean to indicate whether the date and time should be logged or not
        /// </summary>
        private readonly bool _logDate;

        /// <summary>
        /// Initialize a new ApplicationLog object
        /// </summary>
        /// <param name="logDate">A boolean to indicate whether the date and time should be logged or not</param>
        internal ApplicationLog(bool logDate)
        {
            _data = "";
            _logDate = logDate;
        }

        /// <summary>
        /// Add raw data to the log
        /// </summary>
        /// <param name="data">The data that should be added to the raw log</param>
        internal void AddData(string data)
        {
            _data += Environment.NewLine;
            _data += _logDate ? "[" + DateTime.Now.ToString(CultureInfo.CurrentCulture) + "]" + data : data;
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
