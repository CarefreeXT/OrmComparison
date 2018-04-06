using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMPerformanceComparison.Models
{
    public class EFCoreContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Utility.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Warehouse>().HasKey(e => new { e.Id, e.Number });
            builder.Entity<Product>().Property(a => a.Id).UseSqlServerIdentityColumn();

            builder.Entity<Order>()
               .HasOne(a => a.Customer)
               .WithMany(a => a.Orders)
               .HasForeignKey(a => a.CustomerId);

            builder.Entity<OrderDetail>()
                .HasOne(a => a.Product)
                .WithMany()
                .HasForeignKey(a => a.ProductId);

            builder.Entity<Order>()
              .HasMany(a => a.Details)
              .WithOne(a => a.Order)
              .HasForeignKey(a => a.OrderId);
        }
    }
}
