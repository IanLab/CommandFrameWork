using CommandCore.Data;

namespace CommandCore.Execute
{
    public interface IEntityRepositoryFactory
    {
        IEntityRepository GetRepository(Command cmmd);
    }
}
