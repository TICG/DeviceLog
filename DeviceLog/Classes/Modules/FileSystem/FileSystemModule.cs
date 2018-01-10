using System.IO;
using System.Threading;
using DeviceLog.Classes.Log;

namespace DeviceLog.Classes.Modules.FileSystem
{
    internal sealed class FileSystemModule
    {
        private string _path;
        private string _fileTypes;
        private bool _changed;
        private bool _created;
        private bool _deleted;
        private bool _renamed;
        private bool _subDirectories;

        private readonly FileSystemLog _fileSystemLog;
        private FileSystemWatcher _fileSystemWatcher;
        private SynchronizationContext context;

        internal FileSystemModule(string path, string fileTypes, bool changed, bool created, bool deleted, bool renamed, bool subDirectories, LogController logController)
        {
            context = SynchronizationContext.Current;
            _path = path;
            _fileTypes = fileTypes;
            _changed = changed;
            _created = created;
            _deleted = deleted;
            _renamed = renamed;
            _subDirectories = subDirectories;

            _fileSystemLog = new FileSystemLog(true);
            logController.AddLog(_fileSystemLog);
        }

        internal void SetPath(string path)
        {
            _path = path;
        }

        internal void SetFileTypes(string fileTypes)
        {
            _fileTypes = fileTypes;
        }

        internal void SetChangedListener(bool listener)
        {
            _changed = listener;
        }

        internal void SetCreatedListener(bool listener)
        {
            _created = listener;
        }

        internal void SetDeletedListener(bool listener)
        {
            _deleted = listener;
        }

        internal void SetRenamedListener(bool listener)
        {
            _renamed = listener;
        }

        internal void SetIncludeSubDirectories(bool subDirectories)
        {
            _subDirectories = subDirectories;
        }

        internal void Start()
        {
            _fileSystemWatcher = new FileSystemWatcher(_path, _fileTypes) {IncludeSubdirectories = _subDirectories};

            if (_changed)
            {
                _fileSystemWatcher.Changed += OnChanged;
            }

            if (_created)
            {
                _fileSystemWatcher.Created += OnChanged;
            }

            if (_deleted)
            {
                _fileSystemWatcher.Deleted += OnChanged;
            }

            if (_renamed)
            {
                _fileSystemWatcher.Renamed += OnRenamed;
            }
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        internal void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher = null;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            context.Post(val => _fileSystemLog.AddData("File: " + e.FullPath + " " + e.ChangeType), source);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            context.Post(val => _fileSystemLog.AddData("File: " + e.OldFullPath + " renamed to " + e.FullPath), source);
        }
    }
}
