using Microsoft.EntityFrameworkCore;
using Repositories.Configuration;
using Repositories.Models;

namespace Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<OrderBook> OrderBooks { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderBookConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}
