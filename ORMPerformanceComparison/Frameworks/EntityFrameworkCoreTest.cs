using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ORMPerformanceComparison.Models;

namespace ORMPerformanceComparison.Frameworks
{
    class EntityFrameworkCoreTest : IPerformanceTest
    {
        public string Framework => "EntityFrameworkCore";
        
        public long GetCustomerById(int id)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFCoreContext())
                {
                    var data = db.Customers.Find(id);
                }
            });
        }

        public long GetDetailsByOrder(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFCoreContext())
                {
                    var data = db.OrderDetails.Where(a => a.OrderId == orderId).ToArray();
                }
            });
        }

        public long GetOrderAndDetails(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFCoreContext())
                {
                    var data = db.Orders.Include("Details").Where(a => a.Id == orderId).FirstOrDefault();
                }
            });
        }

        public long InsertDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFCoreContext())
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
                using (var db = new EFCoreContext())
                {
                    foreach (var p in products)
                    {
                        db.Products.Add(p);
                    }
                    db.SaveChanges();
                }
            });
        }

        public long UpdateDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFCoreContext())
                {
                    db.Customers.UpdateRange(customers);
                    db.SaveChanges();
                }
            });
        }

        public long DeleteDiscreteDetails(OrderDetail[] details)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFCoreContext())
                {
                    db.OrderDetails.RemoveRange(details);
                    db.SaveChanges();
                }
            });
        }

        public long DeleteDiscreteWarehouses(Warehouse[] warehouses)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new EFCoreContext())
                {
                    db.Warehouses.RemoveRange(warehouses);
                    db.SaveChanges();
                }
            });
        }
    }
}
