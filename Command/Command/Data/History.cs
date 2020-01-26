using System;

namespace CommandCore.Data
{
    public class History
    {
        public string EntityTypeName { get; set; }
        public Guid EntityId { get; set; }
        public string PreValue { get; set; }
        public string CurValue { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string UpdatedUser { get; set; }
    }
}
