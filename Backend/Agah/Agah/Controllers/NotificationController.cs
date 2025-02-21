using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetNotifications/{userId}/{alarmType}")]
        public async Task<IActionResult> GetNotifications(int userId, string alarmType)
        {
            try
            {
                switch (alarmType)
                {
                    case "Alert":
                        var notificationsList = await _unitOfWork.Notification_UserRepository.GetAllByFilterAsync(u => u.UserId == userId, includeProperties: u => u.User);
                        return Ok(notificationsList.OrderByDescending(u => u.CreatedAt));
                    default:
                        break;
                }

                return BadRequest(new { statusMessage = "امکان ارسال اعلان با روش انتخاب شده وجود ندارد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { statusMessage = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" });
            }
        }

        [HttpGet("ReadAllNotifications/{userId}")]
        public async Task<IActionResult> ReadAllNotifications(int userId)
        {
            try
            {
                // TODO : check user token with user id
                var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Id == userId);

                if(user == null)
                    return BadRequest(new { statusMessage = "کاربر مورد نظر یافت نشد" });

                var notificationsList = await _unitOfWork.Notification_UserRepository.GetAllByFilterAsync(u => u.UserId == userId);

                if (notificationsList == null || notificationsList.Count() == 0)
                    return BadRequest(new { statusMessage = "اعلانی برای کاربر مورد نظر یافت نشد" });

                foreach (var notification in notificationsList)
                {
                    notification.IsRead = true;
                    await _unitOfWork.Notification_UserRepository.UpdateAsync(notification);
                }

                await _unitOfWork.SaveAsync();
                return Ok(notificationsList);

            }
            catch (Exception ex)
            {
                return BadRequest(new { statusMessage = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" });
            }
        }
    }

    public class Notification_MessageOptions
    {
        public string MessageSubject { get; set; }
        public string MessageContent { get; set; }
        public string NotificationType { get; set; }
        public User User { get; set; }
    }
}
