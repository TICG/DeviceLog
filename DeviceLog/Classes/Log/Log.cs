namespace DeviceLog.Classes.Log
{
    /// <summary>
    /// Abstract representation of a Log
    /// </summary>
    public abstract class Log : ILogMethods
    {
        /// <summary>
        /// The type of log
        /// </summary>
        internal LogType LogType {get;set;}
        internal delegate void LogUpdatedEventDelegate(string key);

        internal LogUpdatedEventDelegate LogUpdatedEvent;

        internal string Data;

        public void AddData(string data)
        {
            Data += data;
        }

        public string GetData()
        {
            return Data;
        }
    }

    /// <summary>
    /// An enumeration of all available log types
    /// </summary>
    public enum LogType
    {
        Application,
        Keyboard,
        Clipboard,
        FileSystem,
        Monitor
    }
}
