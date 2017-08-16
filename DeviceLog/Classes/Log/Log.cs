
namespace DeviceLog.Classes.Log
{
    /// <summary>
    /// Abstract representation of an internal Log
    /// </summary>
    internal abstract class Log
    {
        /// <summary>
        /// The type of log
        /// </summary>
        internal LogType LogType {get;set;}
    }

    /// <summary>
    /// An enumeration of all available log types
    /// </summary>
    internal enum LogType
    {
        Application,
        Keyboard,
        Clipboard,
        FileSystem,
        Screenshot,
        Network
    }
}
