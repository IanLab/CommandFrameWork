using CommandCore.Data;
using System;
using System.IO;


namespace CommandCore.BroadcastBySharedFolder
{
    public sealed class CommandSubscriber:IDisposable
    {
        private readonly ShareFolderBase _shareFolder;
        private readonly ICommandJsonSerializer _ser;

        private readonly FileSystemWatcher _folderWatcher;

        private readonly CommandFileFilter _filter;

        public event EventHandler<Command> Received;

        public CommandSubscriber(ShareFolderBase shareFolder, ICommandJsonSerializer ser, CommandFileFilter filter)
        {
            _shareFolder = shareFolder ?? throw new ArgumentNullException(nameof(shareFolder));
            _ser = ser ?? throw new ArgumentNullException(nameof(ser));
            _filter = filter;

            _folderWatcher = new FileSystemWatcher
            {
                Path = _shareFolder.GetPath(),
                Filter = $"*{CommandNameHelper.CommandFileExtensionName}"
            };
            _folderWatcher.Created += FolderWatcher_Created;
        }

        public void Start()
        {
            _folderWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _folderWatcher.EnableRaisingEvents = false;
        }

        private void FolderWatcher_Created(object sender, FileSystemEventArgs e)
        {            
            if(_filter.IsInterested(e.Name))
            {
                var cmmdContent = File.ReadAllText(e.FullPath);
                var cmmd = _ser.Deserialize(cmmdContent);
                Received?.Invoke(this,cmmd);
            }
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(!_disposed)
                {
                    _folderWatcher.Created -= FolderWatcher_Created;
                    _folderWatcher.Dispose();                    
                    _disposed = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CommandSubscriber()
        {
            Dispose(false);
        }
    }
}
