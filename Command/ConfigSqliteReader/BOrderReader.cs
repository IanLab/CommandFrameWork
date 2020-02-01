using OrderDTO;
using OrderSqliteDb;
using OrderEntity;
using Microsoft.EntityFrameworkCore;

namespace SqliteReader
{
    public class BOrderReader : CertainTypeOrderReaderBase<BOrderDTO, BOrderEntity>
    {
        protected override DbSet<BOrderEntity> GetCertainOrderDbSet(OrderDbContext dbContext)
        {
            return dbContext.BOrders;
        }
    }
}
