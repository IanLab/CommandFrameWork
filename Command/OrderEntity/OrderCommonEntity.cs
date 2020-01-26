using CommandCore.Data;
using System;

namespace OrderEntity
{
    public class OrderCommonEntity : ICommandAble
    {
        public string No { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public double Amount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAccount { get; set; }
        public string CustomerEmail { get; set; }
        public string Owner { get; set; }
        public Guid Id { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
