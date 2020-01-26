using CommandCore.Data;

namespace CommandCore.Execute
{
    public interface IHistoryRepositoryFactory
    {
        IHistoryRepository GetRepository(Command cmmd);
    }
}
