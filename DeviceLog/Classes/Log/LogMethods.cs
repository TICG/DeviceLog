namespace DeviceLog.Classes.Log
{
    internal interface ILogMethods
    {
        void AddData(string data);
        string GetData();
    }
}
