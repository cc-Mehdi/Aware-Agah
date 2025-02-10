using Datalayer.Data;
using Datalayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Datalayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, Object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
            
            return query.ToList();
        }

        public IEnumerable<T> GetAllByFilter(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, Object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            
            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
            
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, Object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            
            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
