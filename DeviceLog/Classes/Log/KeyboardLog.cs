using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceLog.Classes.Log
{

    internal class KeyboardLog : Log
    {
        private string data;

        internal KeyboardLog()
        {
            data = "";
        }

        internal void AddKey(String key)
        {
            Console.Write(key);
            data += key;
        }

        internal string GetLog()
        {
            return data;
        }
    }
}
