using System;
using System.Runtime.Serialization;

namespace CommandCore.Execute
{
    [Serializable]
    internal class ReferenceObsoleteException : Exception
    {
        private readonly string entityTypeName;
        private readonly Guid id;
        private readonly DateTime expectLastUpdateDateTime;
        private readonly string actualLastUpdatedUser;
        private readonly DateTime actualLastUpdateDateTime;

        public ReferenceObsoleteException()
        {
        }

        public ReferenceObsoleteException(string message) : base(message)
        {
        }

        public ReferenceObsoleteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ReferenceObsoleteException(string entityTypeName, 
            Guid id, 
            DateTime expectLastUpdateDateTime, 
            string actualLastUpdatedUser, 
            DateTime actualLastUpdateDateTime)
            :base($"The reference data '{entityTypeName}' '{id}' you used was obsolete. It was updated by '{actualLastUpdatedUser}' at '{actualLastUpdateDateTime}'.")
        {
            this.entityTypeName = entityTypeName;
            this.id = id;
            this.expectLastUpdateDateTime = expectLastUpdateDateTime;
            this.actualLastUpdatedUser = actualLastUpdatedUser;
            this.actualLastUpdateDateTime = actualLastUpdateDateTime;
        }

        protected ReferenceObsoleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string EntityTypeName => entityTypeName; 

        public DateTime ExpectLastUpdateDateTime => expectLastUpdateDateTime;

        public string ActualLastUpdatedUser => actualLastUpdatedUser;

        public DateTime ActualLastUpdateDateTime => actualLastUpdateDateTime;

        public Guid Id1 => id;
    }
}