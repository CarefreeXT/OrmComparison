using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Transactions;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace ORMPerformanceComparison
{
    public static class Utility
    {
        static Utility()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OrderManagement"].ConnectionString;
        }

        public static long Watch(Action action)
        {
            var watch = Stopwatch.StartNew();
            using (var tran = new TransactionScope(TransactionScopeOption.Required))
            {
                action();
            }
            watch.Stop();
            return watch.ElapsedTicks;
        }

        public static string ConnectionString { get; }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static SqlDataAdapter CreateAdapter(string sql, DbConnection con)
        {
            return new SqlDataAdapter(sql, (SqlConnection)con);
        }
    }
}
