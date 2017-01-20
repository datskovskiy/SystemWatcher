using System;
using System.IO;

namespace DataModelLib
{
    public class SystemWatcher
    {
        private FileSystemWatcher _watcher;
        private StreamWriter _writer;

        public string PathWatcher { get; set; }
        public string FileType { get; set; }
        public bool IncludeSubDirectories { get; set; }
        public string LogFileName { get; set; }

        public SystemWatcher(string path, string fileType, bool includeSubdirectories, string logFileName)
        {
            PathWatcher = path;
            FileType = fileType;
            IncludeSubDirectories = includeSubdirectories;
            LogFileName = logFileName;
        }

        public SystemWatcher(string path): this(path, "*", false, "DirectoryMonitoring.log")
        {
        }

        public SystemWatcher()
            : this(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)))
        {
        }

        public void StartWatch()
        {
            _watcher = new FileSystemWatcher
            {
                Path = PathWatcher,
                IncludeSubdirectories = IncludeSubDirectories,
                Filter = "*." + FileType,
                NotifyFilter = NotifyFilters.FileName
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Size
                               | NotifyFilters.DirectoryName
            };

            _watcher.Changed += OnChanged;
            _watcher.Deleted += OnDeleted;
            _watcher.Created += OnCreated;
            _watcher.Renamed += OnRenamed;
            
            _watcher.EnableRaisingEvents = true;
        }
 
        public void StopWatch()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();    
            }          
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            WriteToLog("File renamed: " + e.OldFullPath + ", New name: " + e.FullPath);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            WriteToLog("File created or added: " + e.FullPath);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            WriteToLog("File deleted: " + e.FullPath);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            WriteToLog("File changed: " + e.FullPath);
        }

        private void WriteToLog(string message)
        {
            if (string.IsNullOrWhiteSpace(LogFileName)) throw new ArgumentException("Error! Incorrect logfile name.");
            if (_writer == null)
            {
                _writer = new StreamWriter(LogFileName, true);
            }
            _writer.WriteLine(message);
            _writer.Flush();
        }
    }
}
