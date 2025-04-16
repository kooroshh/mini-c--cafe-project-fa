using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cafe.DataLayer.Repositories;
using Cafe.ViewModels.Orders;
using Cafe.ViewModels.Product;

namespace Cafe.DataLayer.Services
{
    public class OrderRepository : GenericRepository<Order>, IOrdersRepository
    {
        public OrderRepository(Cafe_DBEntities db) : base(db){}

        public void DeleteOrderProduct(int productId, int orderId)
        {
            Orders_Products orderProduct = this._db.Orders_Products.SingleOrDefault(op => op.ProductID == productId && op.OrderID == orderId);
            this._db.Orders_Products.Remove(orderProduct);
        }

        public void DeleteWithProducts(object key)
        {
            var ordersProducts = this._db.Orders_Products.Where(op => op.OrderID == (int)key).ToList();
            foreach (Orders_Products item in ordersProducts)
            {
                this._db.Orders_Products.Remove(item);
            }
            this.Delete((int)key);
        }

        public List<ProductsListViewModel> GetOrderProducts(int orderId, string filter = "")
        {
            return this._db.Orders_Products.Where(op => op.OrderID == orderId).Select(p => new ProductsListViewModel()
            {
                ProductID = p.ProductID,
                ProductName = p.Product.ProductName,
                ProductPrice = p.Product.ProductPrice,
                Count = p.Count,
                TotalPrice = (p.Count) * (p.Product.ProductPrice)
            }).ToList();
        }

        public List<OrdersListViewModel> GetOrders(Expression<Func<OrdersListViewModel, bool>> where = null, Func<IQueryable<OrdersListViewModel>, IOrderedQueryable<OrdersListViewModel>> orderby = null)
        {
            IQueryable<OrdersListViewModel> query =  this._dbSet.Select(o => new OrdersListViewModel()
            {
                CustomerID = o.CustomerID,
                CustomerName = o.Customer.CustomerName,
                OrderDate = o.OrderDate,
                OrderCode = o.OrderCode,
                OrderID = o.OrderID,
                TotalAmount = o.TotalAmount
            });

            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderby != null)
            {
                query = orderby(query);
            }


            return query.ToList();
        }

    }
}
