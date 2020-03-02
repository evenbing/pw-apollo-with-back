using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PW.DataAccess.Interfaces;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PW.DataAccess.Repositories
{
    public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        protected TContext _dbContext;

        public BaseRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
        {            
            return await _dbContext.Set<TEntity>().ToListAsync();
        }
        
        public virtual async Task<long> CountAsync()
        {            
            return await _dbContext.Set<TEntity>().CountAsync();
        }
        
        public virtual async Task<IReadOnlyCollection<TEntity>> AllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {            
            var query = GetQueryWithProperties(includeProperties);
            return await query.ToListAsync();
        }
        
        public virtual async Task<TEntity> GetSingleAsync(long id)
        {            
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        
        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {            
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        
        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {            
            var query = GetQueryWithProperties(includeProperties);
            return await query.Where(predicate).FirstOrDefaultAsync();
        }
        
        public virtual async Task<IReadOnlyCollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {            
            return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {            
            EntityEntry dbEntityEntry = _dbContext.Entry(entity);
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {            
            EntityEntry dbEntityEntry = _dbContext.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {            
            EntityEntry dbEntityEntry = _dbContext.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteByIdAsync(long id)
        {
            await DeleteWhereAsync(t => t.Id.Equals(id));
        }

        public virtual async Task DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {            
            IEnumerable<TEntity> entities = _dbContext.Set<TEntity>().Where(predicate);

            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            await _dbContext.SaveChangesAsync();
        }
        
        private IQueryable<TEntity> GetQueryWithProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }
    }
}
