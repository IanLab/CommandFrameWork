namespace CommandCore.BroadcastBySharedFolder
{
    public class CommandFileFilter
    {
        public virtual bool IsInterested(string cmdFileName)
        {
            return true;
        }
    }
}
