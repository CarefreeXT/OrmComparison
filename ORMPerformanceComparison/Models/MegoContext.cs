using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caredev.Mego;
using Caredev.Mego.Resolve.Operates;

namespace ORMPerformanceComparison.Models
{
    public class MegoContext : DbContext
    {
        public MegoContext()
            : base("OrderManagement")
        { }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        public void InitialData()
        {
            int productcount = 9000;
            int customercount = 10000;
            int warehousescount = 5000;
            int ordercount = 100000;

            if (this.Products.Count() == 0)
            {
                Random r = new Random(DateTime.Now.Millisecond);
                Product[] products = new Product[productcount];
                for (int i = 0; i < products.Length; i++)
                {
                    products[i] = new Product()
                    {
                        Code = "Pro" + i.ToString(),
                        Name = "Product " + i.ToString().PadLeft(4, '0'),
                        Category = r.Next(1, 5),
                        IsValid = ((i / 5) == 0),
                        UpdateDate = DateTime.Now.AddDays(r.Next(-365, 365)),
                    };
                }
                this.Products.AddRange(a => new Product()
                {
                    Code = a.Code,
                    Name = a.Name,
                    Category = a.Category,
                    IsValid = a.IsValid,
                    UpdateDate = DateTime.Now,
                }, products);

                Customer[] customers = new Customer[customercount];
                for (int i = 0; i < customers.Length; i++)
                {
                    customers[i] = new Customer()
                    {
                        Id = i + 1,
                        Code = "C" + i.ToString().PadLeft(2, '0'),
                        Name = "Customer " + i.ToString().PadLeft(3, '0'),
                        Zip = r.Next(100, 400).ToString(),
                        Address1 = "Address Master " + r.NextDouble().ToString(),
                        Address2 = "This is data  " + r.NextDouble().ToString(),
                    };
                }
                this.Customers.AddRange(customers);

                List<Warehouse> warehouses = new List<Warehouse>();
                for (int i = 0; i < warehousescount; i++)
                {
                    var count = r.Next(3, 10);
                    for (int j = 0; j < count; j++)
                    {
                        warehouses.Add(new Warehouse()
                        {
                            Id = i,
                            Number = j,
                            Name = "Warehouse" + i.ToString().PadLeft(3, '0') + j.ToString().PadLeft(4, '0'),
                            Address = "Address Master " + r.NextDouble().ToString(),
                        });
                    }
                }
                this.Warehouses.AddRange(warehouses);

                Order[] orders = new Order[ordercount];
                for (int i = 0; i < orders.Length; i++)
                {
                    orders[i] = new Order()
                    {
                        Id = i + 1,
                        CreateDate = DateTime.Now.AddDays(r.Next(-365, 365)),
                        ModifyDate = DateTime.Now.AddDays(r.Next(-365, 365)),
                        State = r.Next(1, 10)
                    };
                    this.Orders.Add(orders[i]);
                    this.Orders.AddRelation(orders[i], a => a.Customer, customers[r.Next(0, customers.Length - 1)]);

                    var count = r.Next(3, 10);
                    for (int j = 0; j < count; j++)
                    {
                        var detail = new OrderDetail()
                        {
                            Quantity = r.Next(100, 500),
                            Discount = r.Next(1, 100),
                            Price = Convert.ToDecimal(r.NextDouble() * 100)
                        };
                        this.OrderDetails.Add(detail);
                        this.Orders.AddRelation(orders[i], a => a.Details, detail);
                        this.OrderDetails.AddRelation(detail, a => a.Product, products[r.Next(0, products.Length - 1)]);
                    }
                }
                this.Executor.Execute();
            }
        }
        public void InitialTable()
        {
            var manager = this.Database.Manager;
            var list = new List<DbOperateBase>()
            {
                manager.CreateTable<Order>(),
                manager.CreateTable<OrderDetail>(),
                manager.CreateTable<Customer>(),
                manager.CreateTable<Product>(),
                manager.CreateTable<Warehouse>()
            };
            list.AddRange(manager.CreateRelation((Order o) => o.Customer));
            list.AddRange(manager.CreateRelation((Order o) => o.Details));
            list.AddRange(manager.CreateRelation((OrderDetail o) => o.Product));
            this.Executor.Execute(list);
        }
    }
}
