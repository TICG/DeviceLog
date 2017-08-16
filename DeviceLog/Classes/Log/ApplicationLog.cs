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
        private readonly string _data;

        /// <summary>
        /// Initialize a new ApplicationLog object
        /// </summary>
        /// <param name="dateTime">A boolean to indicate whether the date and time should be logged or not</param>
        /// <param name="data">The data that should be added to the log</param>
        internal ApplicationLog(bool dateTime, string data)
        {
            _data = "";

            if (dateTime)
            {
                _data += "[" + DateTime.Now.ToString(CultureInfo.CurrentCulture) + "]";
                _data += data;

            }
            else
            {
                _data += data;
            }
        }

        /// <summary>
        /// Get the current raw string data of the log
        /// </summary>
        /// <returns>The raw string data of the log</returns>
        internal string GetLog()
        {
            return _data;
        }
    }
}
