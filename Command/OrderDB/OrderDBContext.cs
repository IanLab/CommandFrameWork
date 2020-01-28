using Microsoft.EntityFrameworkCore;
using OrderEntity;

namespace OrderSqliteDb
{

    public class OrderDbContext : DbContext
    {
        public virtual DbSet<OrderCommonEntity> OrderCommons { get; set; }
        public virtual DbSet<AOrderEntity> AOrders { get; set; }
        public virtual DbSet<BOrderEntity> BOrders { get; set; } 
    }
}
