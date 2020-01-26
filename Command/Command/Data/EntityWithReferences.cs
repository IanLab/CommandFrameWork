using System.Collections.Generic;

namespace CommandCore.Data
{
    public class EntityWithReferences
    {
        public ICommandAble Entity { get; set; }
        public IEnumerable<ICommandAble> References { get; set; }
    }
}
