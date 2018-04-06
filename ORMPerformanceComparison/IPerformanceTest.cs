using ORMPerformanceComparison.Models;

namespace ORMPerformanceComparison
{
    /// <summary>
    /// 性能测试项目
    /// </summary>
    public interface IPerformanceTest
    {
        /// <summary>
        /// 框架名称。
        /// </summary>
        string Framework { get; }
        /// <summary>
        /// 随机获取一个客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        long GetCustomerById(int id);
        /// <summary>
        /// 随机获取一个订单的所有明细
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        long GetDetailsByOrder(int orderId);
        /// <summary>
        /// 随机获取一个订单及所有明细
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        long GetOrderAndDetails(int orderId);
        /// <summary>
        /// 插入离散的N个客户。
        /// </summary>
        /// <returns></returns>
        long InsertDiscreteCustomers(Customer[] customers);
        /// <summary>
        /// 插入离散的N个产品，自增主键。
        /// </summary>
        /// <returns></returns>
        long InsertDiscreteProducts(Product[] products);
        /// <summary>
        /// 更新离散的N个客户。
        /// </summary>
        /// <returns></returns>
        long UpdateDiscreteCustomers(Customer[] customers);
        /// <summary>
        /// 删除离散的N个明细。
        /// </summary>
        /// <returns></returns>
        long DeleteDiscreteDetails(OrderDetail[] details);
        /// <summary>
        /// 删除离散的N个仓库，多主键。
        /// </summary>
        /// <returns></returns>
        long DeleteDiscreteWarehouses(Warehouse[] warehouses);
    }
}
