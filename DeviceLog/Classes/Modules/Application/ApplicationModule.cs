using DeviceLog.Classes.Log;

namespace DeviceLog.Classes.Modules.Application
{
    /// <summary>
    /// A class to enable the collection of application logs
    /// </summary>
    internal sealed class ApplicationModule
    {
        /// <summary>
        /// The application log that contains all log data
        /// </summary>
        private readonly ApplicationLog _applicationLog;

        /// <summary>
        /// Initialize a new ApplicationModule object
        /// </summary>
        /// <param name="logDate">A boolean to indicate whether the date and time should be logged</param>
        /// <param name="logController">The LogController that holds a repository of all logs</param>
        internal ApplicationModule(bool logDate, LogController logController)
        {
            _applicationLog = new ApplicationLog(logDate);
            logController.AddLog(_applicationLog);
        }

        /// <summary>
        /// Add raw string data to the current ApplicationLog object
        /// </summary>
        /// <param name="data">The raw data that should be added to the ApplicationLog object</param>
        internal void AddData(string data)
        {
            _applicationLog.AddData(data);
        }

        /// <summary>
        /// Get the current ApplicationLog object
        /// </summary>
        /// <returns>The current ApplicationLog object and the data that it has collected</returns>
        internal ApplicationLog GetLog()
        {
            return _applicationLog;
        }
    }
}
