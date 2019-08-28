using System.IO;

namespace MouseTail.Watchers
{
    public class FileWatcher
    {
        public event FileSystemEventHandler OnChanged;

        public event FileSystemEventHandler OnCreated;

        public event FileSystemEventHandler OnDeleted;

        public event FileSystemEventHandler OnRenamed;

        public FileInfo FileInfo;
        public object ListBox;

        public FileWatcher(string fileName)
        {
            this.FileInfo = new FileInfo(fileName);

            // instantiate the object
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

            // event handlers with the events
            fileSystemWatcher.Changed += Changed;
            fileSystemWatcher.Created += Created;
            fileSystemWatcher.Deleted += Deleted;
            fileSystemWatcher.Renamed += Renamed;

            // tell the watcher where to look
            fileSystemWatcher.Path = FileInfo.DirectoryName;
            fileSystemWatcher.Filter = FileInfo.Name;

            // allow events to fire.
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void Changed(object sender, FileSystemEventArgs e)
        {
            FileSystemEventHandler handler = OnChanged;
            handler?.Invoke(this, e);
        }

        public void Created(object sender, FileSystemEventArgs e)
        {
            FileSystemEventHandler handler = OnCreated;
            handler?.Invoke(this, e);
        }

        public void Deleted(object sender, FileSystemEventArgs e)
        {
            FileSystemEventHandler handler = OnDeleted;
            handler?.Invoke(this, e);
        }

        public void Renamed(object sender, RenamedEventArgs e)
        {
            FileSystemEventHandler handler = OnRenamed;
            handler?.Invoke(this, e);
        }
    }
}