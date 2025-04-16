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
        private OrderRepository _orderRepository;
        private GenericRepository<Orders_Products> _ordersProductsRepository;
        private GenericRepository<User> _userRepository;

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

        public OrderRepository OrderRepository
        {
            get
            {
                if (this._orderRepository == null)
                {
                    this._orderRepository = new OrderRepository(this._db);
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

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new GenericRepository<User>(this._db);
                }
                return this._userRepository;
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
