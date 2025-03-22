using Agah.Controllers;
using Agah.Services.Interfaces;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Agah.Services
{
    public class NotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public NotificationService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<ResponseStatus> SendNotification(Notification_MessageOptions messageOptions)
        {
            try
            {
                var alarm = await _unitOfWork.AlarmRepository.GetFirstOrDefaultAsync(u => u.EnglishName == messageOptions.NotificationType);

                if (alarm == null || alarm.IsActive == false)
                    return new ResponseStatus() { StatusCode = 400, StatusMessage = "آلارم انتخاب شده وجود ندارد یا درحال حاضر غیرفعال میباشد" };

                // Fetch the old notifications beyond the latest 10 and remove them
                var notificationUsersList = await _unitOfWork.Notification_UserRepository
                    .GetAllAsync();

                switch (alarm.EnglishName)
                {
                    case "Alert":

                        var notificationsToRemove = notificationUsersList // Ensure this returns IQueryable<T>
                            .Where(n => n.UserId == messageOptions.User.Id) // Filter for the specific user
                            .OrderByDescending(n => n.CreatedAt) // Sort by newest first
                            .Skip(9) // Keep the latest 9, remove the rest
                            .ToList(); // Convert to list for deletion

                        // Remove old notifications if any exist
                        if (notificationsToRemove.Any())
                            _unitOfWork.Notification_UserRepository.RemoveRange(notificationsToRemove);

                        await _unitOfWork.Notification_UserRepository.AddAsync(new Notification_User()
                        {
                            UserId = messageOptions.User.Id,
                            MessageSubject = messageOptions.MessageSubject,
                            MessageContent = messageOptions.MessageContent,
                        });
                        await _unitOfWork.SaveAsync();
                        return new ResponseStatus() { StatusCode = 200, StatusMessage = "عملیات با موفقیت انجام شد" };
                    case "Email":

                        var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Id == messageOptions.User.Id);

                        if (user == null)
                            return new ResponseStatus() { StatusCode = 400, StatusMessage = "کاربر مورد نظر یافت نشد" };

                        if(user.IsEmailVerified != true)
                            return new ResponseStatus() { StatusCode = 400, StatusMessage = "ابتدا ایمیل خود را تایید کنید" };


                        await _emailService.SendEmailAsync(user.Email, messageOptions.MessageSubject, messageOptions.MessageContent, true);
                        break;
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
