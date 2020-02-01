using System;

namespace CommandCore.Data
{
    public interface IHasId
    {
        Guid Id { get; set; }
    }
}
