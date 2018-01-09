namespace DeviceLog.Classes.Log
{
    /// <inheritdoc />
    /// <summary>
    /// A keyboard log class. This contains log information for the keyboard module
    /// </summary>
    internal class KeyboardLog : Log
    {
        /// <summary>
        /// Initialize a new KeyboardLog
        /// </summary>
        internal KeyboardLog()
        {
            LogType = LogType.Keyboard;
            Data = "";
        }

        /// <summary>
        /// Add new data to the current log
        /// </summary>
        /// <param name="key">The data that should be added to the log</param>
        internal new void AddData(string key)
        {
            Data += key;
            LogUpdatedEvent?.Invoke(Data);
        }
    }
}
