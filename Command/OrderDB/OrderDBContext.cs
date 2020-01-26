using Microsoft.EntityFrameworkCore;
using OrderEntity;

namespace OrderSqliteDb
{

    public class OrderDbContext : DbContext
    {
        public virtual DbSet<OrderCommonEntity> OrderCommons { get; set; }
        public virtual DbSet<OrderAEntity> OrderAs { get; set; }
        public virtual DbSet<OrderBEntity> OrderBs { get; set; }
    }
}
