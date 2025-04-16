using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cafe.ViewModels.Orders;
using Cafe.ViewModels.Product;

namespace Cafe.DataLayer.Repositories
{
    internal interface IOrdersRepository
    {
        List<OrdersListViewModel> GetOrders(Expression<Func<OrdersListViewModel, bool>> where = null, Func<IQueryable<OrdersListViewModel>, IOrderedQueryable<OrdersListViewModel>> orderby = null);
        void DeleteWithProducts(object key);
        List<ProductsListViewModel> GetOrderProducts(int orderId, string filter = "");
        void DeleteOrderProduct(int productId, int orderId);
    }
}
