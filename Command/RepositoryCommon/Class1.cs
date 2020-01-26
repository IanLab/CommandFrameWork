using CommandCore.Data;
using CommandCore.Execute;
using OrderDTO;
using System;
using System.Collections.Generic;

namespace RepositoryCommon
{
    public abstract class OrderARepositoryBase : IEntityRepository
    {
        public ICommandAble GetLastUpdatedEntity(OrderADTO orderA)
        {
            throw new NotImplementedException();
        }

        public void GetNewerRefernces(IEnumerable<ICommandAble> references, out IEnumerable<ICommandAble> newerReferences)
        {
            throw new NotImplementedException();
        }

        public void Save(ICommandAble entity, string currentUpdateUser, DateTime currentUpdateDateTime)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }
    }
}
