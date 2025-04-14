using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cafe.DataLayer.Repositories;

namespace Cafe.DataLayer.Services
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected Cafe_DBEntities _db;
        protected DbSet<TEntity> _dbSet;

        public GenericRepository(Cafe_DBEntities db)
        {
            this._db = db;
            this._dbSet = this._db.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            if(this._db.Entry(entity).State == EntityState.Detached)
            {
                this._dbSet.Attach(entity);
            }
            this._db.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete(object key)
        {
            TEntity entity = this.GetById(key);
            this.Delete(entity);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null)
        {
            IQueryable<TEntity> query = this._dbSet;

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

        public TEntity GetById(object key)
        {
            return this._dbSet.Find(key);
        }


        public void Update(TEntity entity)
        {
            this._dbSet.Attach(entity);
            this._db.Entry(entity).State = EntityState.Modified;
        }
    }
}
