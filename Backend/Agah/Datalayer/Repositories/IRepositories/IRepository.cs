using System.Linq.Expressions;

namespace Datalayer.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, Object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, Object>>[] includeProperties);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, Object>>[] includeProperties);
        Task AddAsync(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
