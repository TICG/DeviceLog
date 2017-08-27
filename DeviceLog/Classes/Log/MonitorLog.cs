using System;
using System.Collections.Generic;
using System.Drawing;

namespace DeviceLog.Classes.Log
{
    /// <inheritdoc />
    /// <summary>
    /// A monitor log class. This contains log information for the monitor module
    /// </summary>
    internal class MonitorLog : Log
    {
        /// <summary>
        /// The dictionary collection containing a list of screenshots and the date at which they were created
        /// </summary>
        private readonly Dictionary<DateTime, Bitmap> _screenShots;

        /// <summary>
        /// Initialize a new MonitorLog object
        /// </summary>
        internal MonitorLog()
        {
            LogType = LogType.Monitor;
            _screenShots = new Dictionary<DateTime, Bitmap>();
        }

        /// <summary>
        /// Add a new Bitmap to the monitor repository
        /// </summary>
        /// <param name="bmp">The bitmap that should be added</param>
        internal void AddBitmap(Bitmap bmp)
        {
            _screenShots.Add(DateTime.Now, bmp);
        }

        /// <summary>
        /// Get the collection of screenshots that was collected
        /// </summary>
        /// <returns>The list of screenshots and their creation date that were collected</returns>
        internal Dictionary<DateTime, Bitmap> GetLog()
        {
            return _screenShots;
        }
    }
}
