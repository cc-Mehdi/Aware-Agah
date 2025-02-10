using Datalayer.Models;

namespace Datalayer.Repositories.IRepositories
{
    public interface INotification_UserRepository : IRepository<Notification_User>
    {
        void Update(Notification_User item);
    }
}
