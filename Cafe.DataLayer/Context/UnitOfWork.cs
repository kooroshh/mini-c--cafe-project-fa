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
