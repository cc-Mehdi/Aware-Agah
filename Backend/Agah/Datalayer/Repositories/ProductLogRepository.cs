using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.Repositories
{
    public class ProductLogRepository : Repository<ProductLog>, IProductLogRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductLogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductLog item)
        {
            var objFromDb = _db.ProductLog.FirstOrDefault(u => u.Id == item.Id);
            objFromDb.Product_Id = item.Product_Id;
            objFromDb.Price = item.Price;
            objFromDb.CreatedAt = item.CreatedAt;
        }
    }
}
