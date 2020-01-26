using CommandCore.Data;
using System;
using System.IO;
using NLog;

namespace CommandCore.BroadcastBySharedFolder
{
    public class CommandSender
    {
        private readonly ShareFolderBase _shareFolder;
        private readonly ICommandJsonSerializer _ser;
        private static readonly ILogger _loger = LogManager.GetCurrentClassLogger();

        public CommandSender(ShareFolderBase shareFolder, ICommandJsonSerializer ser)
        {
            _shareFolder = shareFolder ?? throw new ArgumentNullException(nameof(shareFolder));
            _ser = ser ?? throw new ArgumentNullException(nameof(ser));
        }

        public void Send(Command cmmd)
        {
            var fileName = CommandNameHelper.GetFileName(cmmd);
            var filePath = Path.Combine(_shareFolder.GetPath(), fileName);
            var cmmdContent = _ser.Serializer(cmmd);
            File.WriteAllText(filePath, cmmdContent);
            _loger.Info($"Command {cmmd.UpdateEntity.GetType().Name} {cmmd.UpdateEntity.Entity.Id} was send.");
        }
    }
}
