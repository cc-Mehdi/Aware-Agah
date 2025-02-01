using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;

namespace Datalayer.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User item)
        {
            var objFromDb = _db.User.FirstOrDefault(u => u.Id == item.Id);
            objFromDb.Fullname = item.Fullname;
            objFromDb.Email = item.Email;
            objFromDb.IsEmailVerified = item.IsEmailVerified;
            objFromDb.Phone = item.Phone;
            objFromDb.IsPhoneVerivied = item.IsPhoneVerivied;
            objFromDb.HashedPassword = item.HashedPassword;
            objFromDb.CreatedAt = item.CreatedAt;
            objFromDb.DeletedAt = item.DeletedAt;
        }
    }
}
