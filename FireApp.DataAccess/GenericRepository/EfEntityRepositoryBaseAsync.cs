using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FireApp.DataAccess.GenericRepository
{
    public class EfEntityRepositoryBaseAsync<TEntity, TContext> : IEntityRepositoryBaseAsync<TEntity> where TEntity : class, new() where TContext : DbContext, new()
    {

        public void Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            }
        }

        public async Task<IList<TEntity>> GetListAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            using var context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }
    }
}
