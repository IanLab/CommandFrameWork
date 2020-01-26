using CommandCore.Data;

namespace CommandCore.Execute
{
    public interface IHistoryRepository
    {
        void Record(ICommandAble lastUpdatedEntity, ICommandAble updateEntity);
    }
}