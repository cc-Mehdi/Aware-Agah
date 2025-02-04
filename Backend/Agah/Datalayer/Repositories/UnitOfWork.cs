using Datalayer.Data;
using Datalayer.Repositories.IRepositories;

namespace Datalayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db) : base()
        {
            _db = db;
            UserRepository = new UserRepository(_db);
            ProductRepository = new ProductRepository(_db);
            AlarmRepository = new AlarmRepository(_db);
            ProductLogRepository = new ProductLogRepository(_db);
            ReserveRepository = new ReserveRepository(_db);
        }

        public IUserRepository UserRepository{ get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IAlarmRepository AlarmRepository { get; set; }
        public IProductLogRepository ProductLogRepository { get; set; }
        public IReserveRepository ReserveRepository { get; set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
