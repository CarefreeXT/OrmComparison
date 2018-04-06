using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORMPerformanceComparison.Models;

namespace ORMPerformanceComparison.Frameworks
{
    class EntityFrameworkTest : IPerformanceTest
    {
        public string Framework => "EntityFramework";


        public long GetCustomerById(int id)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFContext())
                {
                    var data = db.Customers.Find(id);
                }
            });
        }

        public long GetDetailsByOrder(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFContext())
                {
                    var data = db.OrderDetails.Where(a => a.OrderId == orderId).ToArray();
                }
            });
        }

        public long GetOrderAndDetails(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFContext())
                {
                    var data = db.Orders.Include("Details").Where(a => a.Id == orderId).FirstOrDefault();
                }
            });
        }

        public long InsertDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFContext())
                {
                    db.Customers.AddRange(customers);
                    db.SaveChanges();
                }
            });
        }

        public long InsertDiscreteProducts(Product[] products)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFContext())
                {
                    db.Products.AddRange(products);
                    db.SaveChanges();
                }
            });
        }

        public long UpdateDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFContext())
                {
                    foreach (var c in customers)
                    {
                        db.Customers.Attach(c);
                        db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                }
            });
        }

        public long DeleteDiscreteDetails(OrderDetail[] details)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFContext())
                {
                    foreach (var c in details)
                    {
                        db.OrderDetails.Attach(c);
                    }
                    db.OrderDetails.RemoveRange(details);
                    db.SaveChanges();
                }
            });
        }

        public long DeleteDiscreteWarehouses(Warehouse[] warehouses)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFContext())
                {
                    foreach (var c in warehouses)
                    {
                        db.Warehouses.Attach(c);
                    }
                    db.Warehouses.RemoveRange(warehouses);
                    db.SaveChanges();
                }
            });
        }

    }
}
