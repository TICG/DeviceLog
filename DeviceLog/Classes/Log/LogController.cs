using System.Collections.Generic;
using System.Linq;

namespace DeviceLog.Classes.Log
{
    /// <summary>
    /// This class holds a repository of all available logs
    /// </summary>
    internal class LogController
    {
        /// <summary>
        /// The repository of all available logs that have been collected
        /// </summary>
        private readonly List<Log> _logList;

        /// <summary>
        /// Initialize a new LogController
        /// </summary>
        internal LogController()
        {
            _logList = new List<Log>();
        }

        /// <summary>
        /// Add a new log to the existing log repository
        /// </summary>
        /// <param name="l">The unique log that should be added to the repository</param>
        internal void AddLog(Log l)
        {
            if (l != null)
            {
                _logList.Add(l);
            }
        }

        /// <summary>
        /// Remove a log from the log repository
        /// </summary>
        /// <param name="l">The log that should be removed from the repository</param>
        internal void RemoveLog(Log l)
        {
            if (_logList.Contains(l))
            {
                _logList.Remove(l);
            }
        }

        /// <summary>
        /// Get all available logs
        /// </summary>
        /// <returns>A list of all available logs</returns>
        internal List<Log> GetLogs()
        {
            return _logList;
        }

        /// <summary>
        /// Get all logs that are of the KeyboardLog type
        /// </summary>
        /// <returns>A list of collected Keyboard logs</returns>
        internal List<Log> GetKeyboardLogs()
        {
            return _logList.Where(l => l.LogType == LogType.Keyboard).ToList();
        }

        /// <summary>
        /// Get all logs that are of the FileSystemLog type
        /// </summary>
        /// <returns>A list of collected FileSystem logs</returns>
        internal List<Log> GetFileSystemLogs()
        {
            return _logList.Where(l => l.LogType == LogType.FileSystem).ToList();
        }
    }
}
