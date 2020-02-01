using Microsoft.EntityFrameworkCore;
using OrderEntity;
using System.IO;
using System;
using System.Globalization;

namespace OrderSqliteDb
{
    public class OrderDbContext : DbContext
    {
        public delegate OrderDbContext Factory(DateTime dt);
        public const string FileName = "OrderDbTemplate.sqlite";
        private readonly string _dbFilePath;
        public virtual DbSet<OrderCommonEntity> OrderCommons { get; set; }
        public virtual DbSet<AOrderEntity> AOrders { get; set; }
        public virtual DbSet<BOrderEntity> BOrders { get; set; }
        private static readonly CultureInfo UsCultureInfo = new CultureInfo("en-US");
        private static string GetDbFolder()
        {
            var f = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Db");
            if(Directory.Exists(f) == false)
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
            modelBuilder.Entity<AOrderEntity>().ToTable(nameof(AOrders));
            modelBuilder.Entity<BOrderEntity>().ToTable(nameof(BOrders));
        }

        public static string GetDbFilePath(DateTime dt)
        {
            return Path.Combine(GetDbFolder(), $"OrderDB{dt.ToString("M/d/yyyy", UsCultureInfo)}.sqlite");
        }

        public static string GetTodayDbFilePath()
        {
            return GetDbFilePath(DateTime.Now);
        }


        public OrderDbContext(DateTime dt = default(DateTime))
        {
            var folder = GetDbFolder();
            var templateFile = Path.Combine(folder, FileName);
            if(dt == default)
            {
                _dbFilePath = templateFile;
            }
            else
            {
                var todayDbFilePath = GetTodayDbFilePath();
                if(File.Exists(todayDbFilePath) == false)
                {
                    File.Copy(templateFile, todayDbFilePath);
                }
                _dbFilePath = todayDbFilePath;
            }
        }
    }
}
