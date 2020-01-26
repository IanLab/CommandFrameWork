using System;
using System.Runtime.Serialization;

namespace CommandCore.Execute
{
    [Serializable]
    internal class DataUpdateConflictException : Exception
    {
        public DataUpdateConflictException()
        {
        }

        //public DataUpdateConcurrencyException(string message) : base(message)
        //{
        //}

        //public DataUpdateConcurrencyException(string message, Exception innerException) : base(message, innerException)
        //{
        //}

        public DataUpdateConflictException(string name, 
            Guid id, 
            DateTime baseOnlastUpdateDateTime, 
            DateTime lastUpdateDateTime, 
            string lastUpdatedUser)
            :base($"The data '{name}' '{id}' you edit was base on the data was updated at '{baseOnlastUpdateDateTime}'. But it was updated by '{lastUpdatedUser}' at '{lastUpdateDateTime}'.")
        {
            this.EntityName = name;
            this.Id = id;
            this.BaseOnLastUpdateDateTime = baseOnlastUpdateDateTime;
            this.LastUpdateDateTime = lastUpdateDateTime;
            this.LastUpdatedUser = lastUpdatedUser;
        }

        //protected DataUpdateConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //}

        public string EntityName { get;  }
        public Guid Id { get; }
        public DateTime BaseOnLastUpdateDateTime { get;}
        public DateTime LastUpdateDateTime { get;  }
        public string LastUpdatedUser { get;  }
    }
}