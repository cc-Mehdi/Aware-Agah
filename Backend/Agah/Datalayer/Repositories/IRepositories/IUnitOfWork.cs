﻿
namespace Datalayer.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; set; }
        IProductRepository ProductRepository {get; set;}
        IAlarmRepository AlarmRepository {get; set;}
        IProductLogRepository ProductLogRepository {get; set;}
        IReserveRepository ReserveRepository {get; set;}
        INotification_UserRepository Notification_UserRepository { get; set; }
        Task SaveAsync();
    }
}
