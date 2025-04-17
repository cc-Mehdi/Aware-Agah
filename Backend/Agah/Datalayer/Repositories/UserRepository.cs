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

        public bool IsUserExist(string userEmail)
        {
            if (_db.User.Any(u => u.Email == userEmail))
                return true;
            return false;
        }

        public async Task UpdateAsync(User item)
        {
            var objFromDb = await _db.User.FirstOrDefaultAsync(u => u.Id == item.Id);

            if (objFromDb == null)
                return;

            objFromDb.Fullname = item.Fullname;
            objFromDb.Email = item.Email;
            objFromDb.IsEmailVerified = item.IsEmailVerified;
            objFromDb.EmailVerificationToken= item.EmailVerificationToken;
            objFromDb.EmailVerificationTokenExpiry = item.EmailVerificationTokenExpiry;
            objFromDb.Phone = item.Phone;
            objFromDb.IsPhoneVerivied = item.IsPhoneVerivied;
            objFromDb.PhoneVerificationToken = item.PhoneVerificationToken;
            objFromDb.PhoneVerificationTokenExpiry = item.PhoneVerificationTokenExpiry;
            objFromDb.HashedPassword = item.HashedPassword;
            objFromDb.Role = item.Role;
            objFromDb.CreatedAt = item.CreatedAt;
            objFromDb.DeletedAt = item.DeletedAt;
        }
    }
}
