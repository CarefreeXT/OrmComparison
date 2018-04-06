using ORMPerformanceComparison.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caredev.Mego;

namespace ORMPerformanceComparison.Frameworks
{
    class MegoTest : IPerformanceTest
    {
        public string Framework => "Mego";
        
        public long GetCustomerById(int id)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new MegoContext())
                {
                    var data = db.Customers.FirstOrDefault(a => a.Id == id);
                }
            });
        }

        public long GetDetailsByOrder(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new MegoContext())
                {
                    var data = db.OrderDetails.Where(a => a.OrderId == orderId).ToArray();
                }
            });
        }

        public long GetOrderAndDetails(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new MegoContext())
                {
                    var data = db.Orders.Include(a => a.Details).Where(a => a.Id == orderId).ToArray();
                }
            });
        }

        public long InsertDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new MegoContext())
                {
                    db.Customers.AddRange(customers);
                    db.Executor.Execute();
                }
            });
        }

        public long InsertDiscreteProducts(Product[] products)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new MegoContext())
                {
                    db.Products.AddRange(products);
                    db.Executor.Execute();
                }
            });
        }

        public long UpdateDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new MegoContext())
                {
                    db.Customers.UpdateRange(customers);
                    db.Executor.Execute();
                }
            });
        }

        public long DeleteDiscreteDetails(OrderDetail[] details)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new MegoContext())
                {
                    db.OrderDetails.RemoveRange(details);
                    db.Executor.Execute();
                }
            });
        }
        
        public long DeleteDiscreteWarehouses(Warehouse[] warehouses)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = new MegoContext())
                {
                    db.Warehouses.RemoveRange(warehouses);
                    db.Executor.Execute();
                }
            });
        }
    }
}
