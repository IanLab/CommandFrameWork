using System;

namespace CommandCore.Data
{
    public interface ICommandAble: IHasId
    {        
        bool IsDeleted { get; set; }
        string LastUpdatedUser { get; set; }
        DateTime LastUpdateDateTime { get; set; }
    }
}
