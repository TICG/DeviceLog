using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceLog.Classes.Log
{
    internal interface ILogMethods
    {
        void AddData(string data);
        string GetData();
    }
}
