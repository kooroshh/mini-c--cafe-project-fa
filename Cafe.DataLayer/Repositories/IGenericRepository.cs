using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.DataLayer.Repositories
{
    internal interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null, params string[] includes);
        TEntity GetById(object key);
        void Delete(TEntity entity);
        void Delete(object key);
        void Update(TEntity entity);
        void Create(TEntity entity);
    }
}
