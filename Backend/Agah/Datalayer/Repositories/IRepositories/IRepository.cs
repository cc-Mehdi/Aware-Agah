using System.Linq.Expressions;

namespace Datalayer.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(params Expression<Func<T, Object>>[] includeProperties);
        IEnumerable<T> GetAllByFilter(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, Object>>[] includeProperties);
        T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, Object>>[] includeProperties);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
