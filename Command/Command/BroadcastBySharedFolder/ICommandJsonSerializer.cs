using CommandCore.Data;


namespace CommandCore.BroadcastBySharedFolder
{
    public interface ICommandJsonSerializer
    {
        string Serializer(Command cmmd);
        Command Deserialize(string str);
    }
}
