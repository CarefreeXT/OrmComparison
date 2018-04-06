using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ORMPerformanceComparison.Models
{
    public class EFContext : DbContext
    {
        public EFContext() : base("name=OrderManagement")
        {
            Database.SetInitializer<EFContext>(null);
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasRequired(a => a.Customer)
                .WithMany(a => a.Orders)
                .HasForeignKey(a => a.CustomerId);

            builder.Entity<OrderDetail>()
                .HasOptional(a => a.Product)
                .WithMany()
                .HasForeignKey(a => a.ProductId);

            builder.Entity<Order>()
              .HasMany(a => a.Details)
              .WithRequired(a => a.Order)
              .HasForeignKey(a => a.OrderId);
        }

    }
}
