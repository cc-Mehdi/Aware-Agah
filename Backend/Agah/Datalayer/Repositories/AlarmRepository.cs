using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;

namespace Datalayer.Repositories
{
    public class AlarmRepository : Repository<Alarm>, IAlarmRepository
    {
        private readonly ApplicationDbContext _db;
        public AlarmRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Alarm item)
        {
            var objFromDb = _db.Alarm.FirstOrDefault(u => u.Id == item.Id);
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
