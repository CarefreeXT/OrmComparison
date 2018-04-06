namespace ORMPerformanceComparison
{
    using ORMPerformanceComparison.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    class Program
    {
        private static int TestInsertCount = 500;
        private static int TestUpdateCount = 500;
        private static int TestDeleteCount = 500;

        private static int TestSelectCount1 = 100;
        private static int TestSelectCount2 = 100;
        private static int TestSelectCount3 = 100;

        private static int TestSumCount = 4;

        static void Main(string[] args)
        {
            InitialDatabase();
            Tuple<int, int> customeIds;
            Tuple<int, int> orderIds;
            var insertCustomes = Enumerable.Range(100000, TestInsertCount).Select(i => new Customer()
            {
                Id = i,
                Name = "Customer " + i.ToString(),
                Code = "C" + i.ToString(),
                Address1 = "A",
                Address2 = "B",
                Zip = "Z"
            }).ToArray();
            var insertProducts = Enumerable.Range(0, TestInsertCount).Select(i => new Product()
            {
                Code = "TP" + i.ToString(),
                Name = "Product Test" + i.ToString(),
                Category = 3,
                IsValid = true,
                UpdateDate = DateTime.Now
            }).ToArray();
            OrderDetail[] deleteDetails;
            Customer[] updateCustomes;
            Warehouse[] deleteWarehouse;
            using (var db = new MegoContext())
            {
                customeIds = Tuple.Create(db.Customers.Min(a => a.Id), db.Customers.Max(a => a.Id));
                orderIds = Tuple.Create(db.Orders.Min(a => a.Id), db.Orders.Max(a => a.Id));
                updateCustomes = db.Customers.Take(TestUpdateCount).ToArray();
                deleteDetails = db.OrderDetails.Take(TestDeleteCount).ToArray();
                deleteWarehouse = db.Warehouses.Take(TestDeleteCount).ToArray();
            }
            foreach (var c in updateCustomes) c.Address1 += "AAAA";

            Random r = new Random(DateTime.Now.Millisecond);
            var frameworks = typeof(Program).Assembly.GetTypes()
                .Where(a => !a.IsAbstract && typeof(IPerformanceTest).IsAssignableFrom(a))
                .Select(b => Activator.CreateInstance(b)).OfType<IPerformanceTest>()
                .ToArray();

            List<TestResultItem> results = new List<TestResultItem>();
            for (int i = 0; i < TestSumCount + 1; i++)
            {
                foreach (var framework in frameworks)
                {
                    foreach (var p in insertProducts) p.Id = 0;
                    var item = new TestResultItem(framework)
                    {
                        Convert.ToInt64(Enumerable.Range(0,TestSelectCount1).Sum(a=>
                            framework.GetCustomerById(r.Next(customeIds.Item1, customeIds.Item2))
                            )),
                        Convert.ToInt64(Enumerable.Range(0,TestSelectCount2).Sum(a=>
                            framework.GetDetailsByOrder(r.Next(orderIds.Item1, orderIds.Item2))
                            )),
                        Convert.ToInt64(Enumerable.Range(0,TestSelectCount3).Sum(a=>
                            framework.GetOrderAndDetails(r.Next(orderIds.Item1, orderIds.Item2))
                            )),
                        framework.InsertDiscreteCustomers(insertCustomes),
                        framework.InsertDiscreteProducts(insertProducts),
                        framework.UpdateDiscreteCustomers(updateCustomes),
                        framework.DeleteDiscreteDetails(deleteDetails),
                        framework.DeleteDiscreteWarehouses(deleteWarehouse),
                    };
                    if (i > 0)
                    {
                        results.Add(item);
                    }
                }
            }
            Output(results);
        }

        static void Output(List<TestResultItem> results)
        {
            var query = from a in results
                        group a by a.Framework into g
                        select new
                        {
                            Framework = g.Key,
                            List = g
                        };
            int spaceCount = 11;
            string headerFormat = string.Join("", Enumerable.Range(0, 8).Select(i => "{" + i.ToString() + ",-" + spaceCount + "}").ToArray());
            string itemFormat = string.Join("", Enumerable.Range(0, 8).Select(i => "{" + i.ToString() + ",-" + spaceCount + ":N2}").ToArray());
            string[] headers = new string[] { "SELECT1", "SELECT2", "SELECT3", "INSERT1", "INSERT2", "UPDATE", "DELETE1", "DELETE2" };

            foreach (var result in query)
            {
                Console.WriteLine(result.Framework.Framework);
                Console.WriteLine(string.Format(headerFormat, headers));
                foreach (var item in result.List)
                {
                    var aa = item.Select(a => TimeSpan.FromTicks(a)).ToArray();
                    var items = aa.Select(a => a.TotalMilliseconds).OfType<object>().ToArray();
                    Console.WriteLine(string.Format(itemFormat, items));
                }
                Console.WriteLine();
            }

            Console.WriteLine("Summary(Average)");
            var padint = query.Max(a => a.Framework.Framework.Length) + 2;
            Console.WriteLine(string.Format(headerFormat, headers));
            foreach (var result in query)
            {
                var list = result.List;
                var data = Enumerable.Range(0, list.First().Count).Select(i =>
                  list.Average(m => TimeSpan.FromTicks(m[i]).TotalMilliseconds)
                ).OfType<object>().ToArray();
                Console.WriteLine(string.Format(itemFormat, data) + result.Framework.Framework);
            }
            Console.ReadKey();
        }
        static void InitialDatabase()
        {
            using (var ef = new Models.EFContext())
            {
                if (!ef.Database.Exists())
                {
                    ef.Database.Create();

                    using (var db = new Models.MegoContext())
                    {
                        db.InitialTable();
                    }
                }
            }
            using (var db = new Models.MegoContext())
            {
                db.InitialData();
            }
        }
    }

    public class TestResultItem : List<long>
    {
        public TestResultItem(IPerformanceTest frame)
        {
            Framework = frame;
        }

        public IPerformanceTest Framework { get; }
    }
}
