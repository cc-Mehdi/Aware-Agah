using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.Repositories
{
    public class ReserveRepository : Repository<Reserve>, IReserveRepository
    {
        private readonly ApplicationDbContext _db;
        public ReserveRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Reserve item)
        {
            var objFromDb = await _db.Reserve.FirstOrDefaultAsync(u => u.Id == item.Id);

            if (objFromDb == null)
                return;

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
