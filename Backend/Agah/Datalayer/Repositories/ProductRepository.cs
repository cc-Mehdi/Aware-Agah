using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;

namespace Datalayer.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product item)
        {
            var objFromDb = _db.Product.FirstOrDefault(u => u.Id == item.Id);
            objFromDb.Title = item.Title;
            objFromDb.CreatedAt = item.CreatedAt;
            objFromDb.DeletedAt = item.DeletedAt;
        }
    }
}
