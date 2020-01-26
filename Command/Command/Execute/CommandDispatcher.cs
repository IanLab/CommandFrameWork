using System;
using System.Collections.Concurrent;
using CommandCore.Data;
using CommandCore.BroadcastBySharedFolder;
using System.Threading.Tasks;
using NLog;

namespace CommandCore.Execute
{
    public sealed class CommandDispatcher:IDisposable
    {
        private readonly CommandExecutor _cmmdExecutor;
        private readonly CommandSubscriber _cmmdSub;
        private readonly BlockingCollection<Command> _queue;
        private readonly Task _consumerTask;
        private readonly static ILogger _logger;
        private bool disposed = false;

        public CommandDispatcher(CommandExecutor cmmdExecutor, 
            CommandSubscriber cmmdSub)
        {
            _cmmdExecutor = cmmdExecutor ?? throw new ArgumentNullException(nameof(cmmdExecutor));
            _cmmdSub = cmmdSub ?? throw new ArgumentNullException(nameof(cmmdSub));
            _queue = new BlockingCollection<Command>();
            _consumerTask = Task.Run(Consume);
            _cmmdSub.Received += CmmdSub_Received;
        }

        private void CmmdSub_Received(object sender, Command e)
        {
            _queue.Add(e);
            _logger.Info($"Command {e.UpdateEntity.Entity.GetType().Name} {e.UpdateEntity.Entity.Id} was received.");
        }

        public void Start()
        {
            _cmmdSub.Start();
        }

        public void Stop()
        {
            _cmmdSub.Stop();
            _queue.CompleteAdding();
            for(int i = 0; i < 5; i++)
            {
                if(_consumerTask.Status == TaskStatus.Running)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        private void Consume()
        {
            while(_queue.IsCompleted)
            {
                var cmmd = _queue.Take();
                _cmmdExecutor.Execute(cmmd);

                _logger.Info($"Command {cmmd.UpdateEntity.Entity.GetType().Name} {cmmd.UpdateEntity.Entity.Id} was executed.");
            }
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(!disposed)
                {
                    _cmmdSub.Received -= CmmdSub_Received;
                    _cmmdSub.Dispose();
                    _queue.Dispose();
                    _consumerTask.Dispose();
                    disposed = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CommandDispatcher()
        {
            Dispose(false);
        }
    }
}
