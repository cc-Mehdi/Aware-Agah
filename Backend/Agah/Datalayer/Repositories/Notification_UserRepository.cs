using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.Repositories
{
    public class Notification_UserRepository : Repository<Notification_User>, INotification_UserRepository
    {
        private readonly ApplicationDbContext _db;
        public Notification_UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Notification_User item)
        {
            var objFromDb = await _db.Notification_User.FirstOrDefaultAsync(u => u.Id == item.Id);

            if (objFromDb == null)
                return;

            objFromDb.UserId = item.UserId;
            objFromDb.MessageSubject = item.MessageSubject;
            objFromDb.MessageContent = item.MessageContent;
            objFromDb.IsRead = item.IsRead;
            objFromDb.CreatedAt = item.CreatedAt;
            objFromDb.DeletedAt = item.DeletedAt;
        }
    }
}
