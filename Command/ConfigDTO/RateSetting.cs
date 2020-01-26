using CommandCore.Data;
using System;
using System.Collections.Generic;

namespace ConfigDTO
{
    public class PeriodRatesDTO: ICommandAble
    {
        public IEnumerable<PeriodRate> Items { get; set; }
        public Guid Id { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PeriodRate
    {
        public int Period { get; set; }
        public double Rate { get; set; }
    }
}
