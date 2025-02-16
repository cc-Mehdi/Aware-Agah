using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface INotification_UserRepository : IRepository<Notification_User>
    {
        Task UpdateAsync(Notification_User item);
    }
}
