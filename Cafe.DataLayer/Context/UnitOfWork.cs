using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe.DataLayer.Repositories;
using Cafe.DataLayer.Services;

namespace Cafe.DataLayer.Context
{
    public class UnitOfWork : IDisposable
    {
        private Cafe_DBEntities _db;
        private GenericRepository<Product> _productsRepository;
        private CustomerRepository _customerRepository;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<Orders_Products> _ordersProductsRepository;

        public GenericRepository<Product> ProductRepository { 
            get 
            {
                if (this._productsRepository == null)
                {
                    this._productsRepository = new GenericRepository<Product>(this._db);
                }
                return this._productsRepository;
            }
        }

        public CustomerRepository CustomerRepository
        {
            get
            {
                if (this._customerRepository == null)
                {
                    this._customerRepository = new CustomerRepository(this._db);
                }
                return this._customerRepository;
            }
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (this._orderRepository == null)
                {
                    this._orderRepository = new GenericRepository<Order>(this._db);
                }
                return this._orderRepository;
            }
        }  

        public GenericRepository<Orders_Products> OrdersProductsRepository
        {
            get
            {
                if (this._ordersProductsRepository == null)
                {
                    this._ordersProductsRepository = new GenericRepository<Orders_Products>(this._db);
                }
                return this._ordersProductsRepository;
            }
        }

        public UnitOfWork()
        {
            this._db = new Cafe_DBEntities();
        }

        public void Save()
        {
            this._db.SaveChanges();
        }
        public void Dispose()
        {
            this._db.Dispose();
        }
    }
}
