using CommandCore.Data;
using System;

namespace OrderEntity
{
    public class BOrderEntity: IHasId
    {
        public Guid Id { get; set; }
        public double OrderBRate { get; set; }
    }
}
