using CommandCore.Data;
using System;
using System.Collections.Generic;

namespace CommandCore.Execute
{
    public interface IEntityRepository:IDisposable
    {
        ICommandAble LastUpdated { get; }
        IEnumerable<ICommandAble> NewerReferences { get; }        
        void Save();
    }
}