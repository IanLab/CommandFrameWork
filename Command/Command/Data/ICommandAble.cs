using System;

namespace CommandCore.Data
{
    public interface ICommandAble
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        string LastUpdatedUser { get; set; }
        DateTime LastUpdateDateTime { get; set; }
    }
}
