using CommandCore.Data;
using System;
using System.Linq;

namespace CommandCore.Execute
{
    public class CommandExecutor
    {
        private readonly IHistoryRepositoryFactory _historyRepositoryFactory;
        private readonly IEntityRepositoryFactory _entityRepositoryFactory;

        public CommandExecutor(IHistoryRepositoryFactory historyRecorder,
                               IEntityRepositoryFactory entityRepository)
        {
            _historyRepositoryFactory = historyRecorder ?? throw new ArgumentNullException(nameof(historyRecorder));
            _entityRepositoryFactory = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
        }

        public event EventHandler<CommandExecutedEventArg> CommandExecuted;

        public void Execute(Command cmd)
        {
            if (cmd is null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            if (BeginExecute(cmd) == false)
            {
                return;
            }
            CommandExecutedEventArg cmmdExecutedEventArg =
                    new CommandExecutedEventArg(cmd.UpdateEntity.Entity);

            try
            {
                var history = _historyRepositoryFactory.GetRepository(cmd);
                IEntityRepository entityRepository = _entityRepositoryFactory.GetRepository(cmd);

                var lastUpdatedEntity = entityRepository.LastUpdated;

                if (lastUpdatedEntity != null)
                {
                    CheckData(cmd, entityRepository, lastUpdatedEntity);
                }

                if (history != null)
                {
                    history.Record(lastUpdatedEntity, cmd.UpdateEntity.Entity);
                }

                entityRepository.Save();

                cmd.UpdateEntity.Entity.LastUpdateDateTime = cmd.CurrentUpdateDateTime;
                cmd.UpdateEntity.Entity.LastUpdatedUser = cmd.CurrentUpdateUser;
            }
            catch (Exception exp)
            when (exp is DataUpdateConflictException || exp is ReferenceObsoleteException)
            {
                cmmdExecutedEventArg.IsSuccess = false;
                cmmdExecutedEventArg.ErrorMessage = exp.Message;
            }

            CommandExecuted?.Invoke(this, cmmdExecutedEventArg);
        }

        private static void CheckData(Command cmd, IEntityRepository entityRepository, ICommandAble lastUpdatedEntity)
        {
            if (lastUpdatedEntity.LastUpdateDateTime > cmd.UpdateEntity.Entity.LastUpdateDateTime)
            {
                throw new DataUpdateConflictException(cmd.UpdateEntity.Entity.GetType().Name,
                    cmd.UpdateEntity.Entity.Id,
                    cmd.UpdateEntity.Entity.LastUpdateDateTime,
                    lastUpdatedEntity.LastUpdateDateTime,
                    lastUpdatedEntity.LastUpdatedUser);
            }

            var newerReferences = entityRepository.NewerReferences;
            if (newerReferences.Any())
            {
                var newerReference = newerReferences.First();
                var reference = cmd.UpdateEntity.References.Single(i => i.Id == newerReference.Id);
                throw new ReferenceObsoleteException(
                    newerReference.GetType().Name,
                    newerReference.Id,
                    reference.LastUpdateDateTime,
                    newerReference.LastUpdatedUser,
                    newerReference.LastUpdateDateTime);
            }
        }

        protected virtual bool BeginExecute(Command cmd)
        {
            return true;
        }

    }
}
