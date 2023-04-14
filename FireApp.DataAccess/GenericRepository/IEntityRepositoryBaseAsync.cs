using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FireApp.DataAccess.GenericRepository
{
    public interface IEntityRepositoryBaseAsync<T> where T : class, new()
    {
        void Add(T entity);
        Task<IList<T>> GetListAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);
    }
}
