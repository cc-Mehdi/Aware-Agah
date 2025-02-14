using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;

namespace Datalayer.Repositories
{
    public class ReserveRepository : Repository<Reserve>, IReserveRepository
    {
        private readonly ApplicationDbContext _db;
        public ReserveRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Reserve item)
        {
            var objFromDb = _db.Reserve.FirstOrDefault(u => u.Id == item.Id);
            objFromDb.User_Id = item.User_Id;
            objFromDb.Product_Id = item.Product_Id;
            objFromDb.Alarm_Id = item.Alarm_Id;
            objFromDb.MinPrice = item.MinPrice;
            objFromDb.MaxPrice = item.MaxPrice;
            objFromDb.IsSent = item.IsSent;
            objFromDb.CreatedAt = item.CreatedAt;
            objFromDb.DeletedAt = item.DeletedAt;
        }
    }
}
