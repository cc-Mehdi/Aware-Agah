using Agah.Controllers;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Agah.Services
{
    public class NotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseStatus> SendNotification(Notification_MessageOptions messageOptions)
        {
            try
            {
                var alarm = _unitOfWork.AlarmRepository.GetFirstOrDefault(u => u.EnglishName == messageOptions.NotificationType);

                if (alarm == null || alarm.IsActive == false)
                    return new ResponseStatus() { StatusCode = 400, StatusMessage = "آلارم انتخاب شده وجود ندارد یا درحال حاضر غیرفعال میباشد" };

                switch (alarm.EnglishName)
                {
                    case "Alert":
                        // Fetch the old notifications beyond the latest 10 and remove them
                        var notificationsToRemove = _unitOfWork.Notification_UserRepository
                            .GetAll() // Ensure this returns IQueryable<T>
                            .Where(n => n.UserId == messageOptions.User.Id) // Filter for the specific user
                            .OrderByDescending(n => n.CreatedAt) // Sort by newest first
                            .Skip(9) // Keep the latest 9, remove the rest
                            .ToList(); // Convert to list for deletion

                        // Remove old notifications if any exist
                        if (notificationsToRemove.Any())
                            _unitOfWork.Notification_UserRepository.RemoveRange(notificationsToRemove);

                        _unitOfWork.Notification_UserRepository.Add(new Notification_User()
                        {
                            UserId = messageOptions.User.Id,
                            MessageSubject = messageOptions.MessageSubject,
                            MessageContent = messageOptions.MessageContent,
                        });
                        await _unitOfWork.SaveAsync();
                        return new ResponseStatus() { StatusCode = 200, StatusMessage = "عملیات با موفقیت انجام شد" };
                    default:
                        break;
                }

                return new ResponseStatus() { StatusCode = 200, StatusMessage = "عملیات با موفقیت انجام شد" };
            }
            catch (Exception ex)
            {
                return new ResponseStatus() { StatusCode = 400, StatusMessage = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" };
            }
        }
    }
}
