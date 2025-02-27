using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(User item)
        {
            var objFromDb = await _db.User.FirstOrDefaultAsync(u => u.Id == item.Id);

            if (objFromDb == null)
                return;

            objFromDb.Fullname = item.Fullname;
            objFromDb.Email = item.Email;
            objFromDb.IsEmailVerified = item.IsEmailVerified;
            objFromDb.Phone = item.Phone;
            objFromDb.IsPhoneVerivied = item.IsPhoneVerivied;
            objFromDb.HashedPassword = item.HashedPassword;
            objFromDb.Role = item.Role;
            objFromDb.CreatedAt = item.CreatedAt;
            objFromDb.DeletedAt = item.DeletedAt;
        }
    }
}
