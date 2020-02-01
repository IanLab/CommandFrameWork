using CommandCore.Data;
using System;

namespace OrderEntity
{
    public class AOrderEntity: IHasId
    {
        public Guid Id { get; set; }
        public string AOrderP1 { get; set; }
    }
}
