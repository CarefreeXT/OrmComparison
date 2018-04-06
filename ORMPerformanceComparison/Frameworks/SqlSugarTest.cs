using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORMPerformanceComparison.Models;
using SqlSugar;
using Dapper;

namespace ORMPerformanceComparison.Frameworks
{
    class SqlSugarTest : IPerformanceTest
    {
        public string Framework => "SqlSugar";

        private SqlSugarClient CreateClient()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Utility.ConnectionString, //必填
                DbType = DbType.SqlServer, //必填
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.SystemTable
            }); //默认SystemTable
            return db;
        }


        public long GetCustomerById(int id)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = CreateClient())
                {
                    var data = db.Queryable<Customer>().First(a => a.Id == id);
                }
            });
        }

        public long GetDetailsByOrder(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = CreateClient())
                {
                    var data = db.Queryable<OrderDetail>().Where(a => a.OrderId == orderId).ToList();
                }
            });
        }

        public long GetOrderAndDetails(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = CreateClient())
                {
                    var list = db.Queryable<OrderDetail, Order>((d, o) => new object[]
                    {
                        JoinType.Inner , d.OrderId==o.Id
                    })
                    .Where(d => d.OrderId == orderId)
                    .Select((d, o) => new { d, o }).ToList();
                }
            });
        }

        public long InsertDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = CreateClient())
                {
                    var t3 = db.Insertable(customers).ExecuteCommand();
                }
            });
        }

        public long InsertDiscreteProducts(Product[] products)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = CreateClient())
                {
                    var t3 = db.Insertable(products).ExecuteCommand();
                }
            });
        }

        public long UpdateDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = CreateClient())
                {
                    var t3 = db.Updateable(customers).ExecuteCommand();
                }
            });
        }

        public long DeleteDiscreteDetails(OrderDetail[] details)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = CreateClient())
                {
                    var t3 = db.Deleteable<OrderDetail>(details.Select(a => a.Id).ToArray()).ExecuteCommand();
                }
            });
        }

        public long DeleteDiscreteWarehouses(Warehouse[] warehouses)
        {
            return Utility.Watch(delegate ()
            {
                using (var db = CreateClient())
                {
                    foreach (var i in warehouses)
                    {
                        var t3 = db.Deleteable(i).ExecuteCommand();
                    }
                }
            });
        }
    }
}
