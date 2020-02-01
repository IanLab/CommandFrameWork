using OrderDTO;
using OrderSqliteDb;
using System;
using System.Collections.Generic;

namespace SqliteReader
{
    public interface ICertainTypeOrderReader
    {
        IEnumerable<OrderBaseDTO> GetOrders(OrderDbContext dbContext);
        OrderBaseDTO GetOrder(Guid id, OrderDbContext dbContext);
    }
}