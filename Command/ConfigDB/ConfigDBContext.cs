using ConfigEntity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;

namespace ConfigSqliteDb
{
    public class ConfigDbContext:DbContext
    {
        private static readonly string _dbFilePath = Path.Combine(GetDbFolder(), "Config.sqlite");
        public virtual DbSet<CustomerEntity> Customers { get; set; }

        private static string GetDbFolder()
        {
            var f = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Db");
            if(!Directory.Exists(f))
            {
                Directory.CreateDirectory(f);
            }
            return f;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbFilePath};");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>().ToTable(nameof(Customers));
        }
    }
}
