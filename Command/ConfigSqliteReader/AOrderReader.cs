using OrderDTO;
using OrderSqliteDb;
using OrderEntity;
using Microsoft.EntityFrameworkCore;

namespace SqliteReader
{
    public class AOrderReader : CertainTypeOrderReaderBase<AOrderDTO,AOrderEntity>
    {
        protected override DbSet<AOrderEntity> GetCertainOrderDbSet(OrderDbContext dbContext)
        {
            return dbContext.AOrders;
        }
    }
}
