using Microsoft.EntityFrameworkCore;

namespace ConfigSqliteDb
{
    public class ConfigDbContext:DbContext
    {
        public virtual DbSet<CustomerEntity> Customers { get; set; }
    }
}
