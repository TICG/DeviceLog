using System;
using System.Globalization;

namespace DeviceLog.Classes.Log
{
    /// <inheritdoc />
    /// <summary>
    /// A class that represent a change in the application
    /// </summary>
    internal class ApplicationLog : Log
    {
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
            LogType = LogType.Application;
            Data = "";
            _logDate = logDate;
        }

        /// <summary>
        /// Add raw data to the log
        /// </summary>
        /// <param name="data">The data that should be added to the raw log</param>
        internal new void AddData(string data)
        {
            if (Data.Length != 0)
            {
                Data += Environment.NewLine;
            }
            Data += _logDate ? "[" + DateTime.Now.ToString(CultureInfo.CurrentCulture) + "]" + data : data;
            LogUpdatedEvent?.Invoke(Data);
        }
    }
}
