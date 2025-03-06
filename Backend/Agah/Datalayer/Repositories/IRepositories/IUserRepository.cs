using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdateAsync(User item);
        bool IsUserExist(string userEmail);
    }
}
