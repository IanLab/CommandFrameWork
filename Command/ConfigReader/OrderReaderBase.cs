using OrderDTO;
using System;
using System.Collections.Generic;

namespace Reader
{
    public abstract class OrderReaderBase
    {
        public abstract IEnumerable<OrderBaseDTO> GetOrders(DateTime dt);
        public IEnumerable<OrderBaseDTO> GetTodayOrders()
        {
            return GetOrders(DateTime.Now);
        }

        public abstract OrderBaseDTO GetOrder(Guid id, string orderTypeName);
    }
}
