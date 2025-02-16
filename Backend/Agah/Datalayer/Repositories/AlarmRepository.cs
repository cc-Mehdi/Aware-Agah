using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.Repositories
{
    public class AlarmRepository : Repository<Alarm>, IAlarmRepository
    {
        private readonly ApplicationDbContext _db;
        public AlarmRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Alarm item)
        {
            var objFromDb = await _db.Alarm.FirstOrDefaultAsync(u => u.Id == item.Id);

            if (objFromDb == null)
                return;

            objFromDb.EnglishName = item.EnglishName;
            objFromDb.PersianName = item.PersianName;
            objFromDb.ShortDescription = item.ShortDescription;
            objFromDb.IsActive = item.IsActive;
            objFromDb.AlarmPrice = item.AlarmPrice;
            objFromDb.CreatedAt = item.CreatedAt;
            objFromDb.DeletedAt = item.DeletedAt;
        }
    }
}
