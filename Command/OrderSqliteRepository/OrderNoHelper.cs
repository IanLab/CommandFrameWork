using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OrderSqliteRepository
{
    internal static class OrderNoHelper
    {
        public static void SetNo(OrderSqliteDb.OrderDbContext dbContext,
            OrderDTO.OrderBaseDTO order)
        {
            var typeName = order.GetType().Name;
            System.Diagnostics.Debug.Assert(string.IsNullOrEmpty(order.No));
            var dtMin = order.StartDate.Date;
            var dtMax = dtMin.AddDays(1);
            var existOrderNo = (from o in dbContext.OrderCommons
                                where o.OrderType == typeName
                                && o.StartDate >= dtMin 
                                && o.StartDate < dtMax
                                select o.No).ToArray();

            for(int i = 0; i < 10000; i++)
            {
                var no = $"{typeName}{i:X4}";
                if(existOrderNo.Contains(no) == false)
                {
                    order.No = no;
                    break;
                }
            }

            if(string.IsNullOrEmpty(order.No))
            {
                throw new Exception();
            }
        }
    }
}
