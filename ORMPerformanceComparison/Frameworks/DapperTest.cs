using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ORMPerformanceComparison.Models;
using DapperExtensions;

namespace ORMPerformanceComparison.Frameworks
{
    class DapperTest : IPerformanceTest
    {
        public string Framework => "Dapper";

        public long GetCustomerById(int id)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    var data = conn.Query<Customer>(@"SELECT  Id ,
        Address1 ,
        Address2 ,
        Code ,
        Name ,
        Zip
FROM    dbo.Customers
WHERE   Id = @Id;", new { Id = id });
                }
            });
        }

        public long GetDetailsByOrder(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    var data = conn.Query<OrderDetail>(
@"SELECT  Id ,
        Discount ,
        [Key] ,
        OrderId ,
        Price ,
        ProductId ,
        Quantity
FROM    dbo.OrderDetails
WHERE   OrderId = @OrderId;", new { OrderId = orderId });
                }
            });
        }

        public long GetOrderAndDetails(int orderId)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    var players = conn.Query<OrderDetail, Order, OrderDetail>(
@"SELECT  od.Id ,
        od.Discount ,
        od.[Key] ,
        od.OrderId ,
        od.Price ,
        od.ProductId ,
        od.Quantity ,
        o.Id OrderId ,
        o.CreateDate ,
        o.CustomerId ,
        o.ModifyDate ,
        o.State
FROM    dbo.OrderDetails od
        INNER JOIN dbo.Orders o ON o.Id = od.OrderId
WHERE   o.Id = @OrderId;", (detail, order) =>
{
    detail.Order = order;
    if (order.Id == 0)
    {
        order.Id = detail.OrderId;
    }
    return detail;
}, splitOn: "OrderId", param: new { OrderId = orderId });
                }
            });
        }

        public long InsertDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();

                    int val = conn.Execute(
    @"INSERT INTO dbo.Customers( Id, Address1, Address2, Code, Name, Zip ) 
VALUES( @Id, @Address1, @Address2, @Code, @Name, @Zip )"
    , customers);
                }
            });
        }

        public long InsertDiscreteProducts(Product[] products)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    conn.Execute(
@"INSERT INTO dbo.Products
        ( Category ,Code ,IsValid ,Name ,UpdateDate)
VALUES
        ( @Category ,@Code ,@IsValid ,@Name ,@UpdateDate)"
, products);
                }
            });
        }

        public long UpdateDiscreteCustomers(Customer[] customers)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    int val = conn.Execute(
@"UPDATE  dbo.Customers
SET     Address1 = @Address1 ,
        Address2 = @Address2 ,
        Code = @Code ,
        Name = @Name ,
        Zip = @Zip
WHERE   Id = @Id;"
, customers);
                }
            });
        }

        public long DeleteDiscreteDetails(OrderDetail[] details)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    int val = conn.Execute(
@"DELETE dbo.OrderDetails WHERE Id=@Id;"
, details);
                }
            });
        }

        public long DeleteDiscreteWarehouses(Warehouse[] warehouses)
        {
            return Utility.Watch(delegate ()
            {
                using (var conn = Utility.CreateConnection())
                {
                    conn.Open();
                    int val = conn.Execute(
@"DELETE dbo.Warehouses WHERE Id=@Id AND Number=@Number;"
, warehouses);
                }
            });
        }
    }
}
