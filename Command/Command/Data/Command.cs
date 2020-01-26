using System;
using System.Collections.Generic;

namespace CommandCore.Data
{

    public class Command
    {
        public Command(
            string currentUpdateUser,
            DateTime currentUpdateDateTime,
            EntityWithReferences updateEntity)
        {
            CurrentUpdateUser = currentUpdateUser ?? throw new ArgumentNullException(nameof(currentUpdateUser));
            CurrentUpdateDateTime = currentUpdateDateTime;
            UpdateEntity = updateEntity ?? throw new ArgumentNullException(nameof(updateEntity));
        }

        public string CurrentUpdateUser { get; set; }
        public DateTime CurrentUpdateDateTime { get; set; }

        public EntityWithReferences UpdateEntity { get; set; }

        
    }
}
