using PW.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PW.DataAccess.Interfaces
{
    public interface IBaseRepository<TEntity>
        where TEntity : BaseEntity        
    {        
        Task<IReadOnlyCollection<TEntity>> GetAllAsync();        
        Task<long> CountAsync();        
        Task<IReadOnlyCollection<TEntity>> AllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);       
        Task<TEntity> GetSingleAsync(long id);        
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate);        
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);        
        Task<IReadOnlyCollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteByIdAsync(long id);
        Task DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate);        
    }
}
