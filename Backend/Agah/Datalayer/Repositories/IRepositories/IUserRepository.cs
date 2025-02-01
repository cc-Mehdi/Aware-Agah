using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User item);
    }
}
