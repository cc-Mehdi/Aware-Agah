using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Product item)
        {
            var objFromDb = await _db.Product.FirstOrDefaultAsync(u => u.Id == item.Id);

            if (objFromDb == null)
                return;

            objFromDb.EnglishName = item.EnglishName;
            objFromDb.PersianName = item.PersianName;
            objFromDb.IconName = item.IconName;
            objFromDb.CreatedAt = item.CreatedAt;
            objFromDb.DeletedAt = item.DeletedAt;
        }
    }
}
