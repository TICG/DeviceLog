using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DeviceLog.Classes.Modules.Window
{
    /// <summary>
    /// A class that can be used to retrieve the title of the currently active window
    /// </summary>
    internal class WindowModule
    {
        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working). The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads
        /// </summary>
        /// <returns>The return value is a handle to the foreground window. The foreground window can be NULL in certain circumstances, such as when a window is losing activation</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Copies the text of the specified window's title bar (if it has one) into a buffer. If the specified window is a control, the text of the control is copied. However, GetWindowText cannot retrieve the text of a control in another application
        /// </summary>
        /// <param name="hWnd">A handle to the window or control containing the text</param>
        /// <param name="text">The buffer that will receive the text. If the string is as long or longer than the buffer, the string is truncated and terminated with a null character</param>
        /// <param name="count">The maximum number of characters to copy to the buffer, including the null character. If the text exceeds this limit, it is truncated</param>
        /// <returns>If the function succeeds, the return value is the length, in characters, of the copied string, not including the terminating null character. If the window has no title bar or text, if the title bar is empty, or if the window or control handle is invalid, the return value is zero</returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        /// <summary>
        /// Get the title of the currently activated window
        /// </summary>
        /// <returns>The string value of the currently activated window</returns>
        internal string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            return GetWindowText(handle, buff, nChars) > 0 ? buff.ToString() : null;
        }
    }
}
