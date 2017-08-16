﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceLog.Classes.Log
{
    internal class ClipboardLog : Log
    {
        private string _data;
        private readonly bool _logDate;

        internal ClipboardLog(bool logDate)
        {
            LogType = LogType.Clipboard;
            _data = "";
            _logDate = logDate;
        }

        internal void AddData(string data)
        {
            _data += Environment.NewLine;
            if (_logDate)
            {
                _data += "[" + DateTime.Now + "]" + data;
            }
            else
            {
                _data += Environment.NewLine;
                _data += data;
            }
        }

        internal string GetLog()
        {
            return _data;
        }
    }
}