using System;

namespace DTOCommon
{
    public abstract class CommandAbleDTO
    {
        public Guid Id { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
    }
}
