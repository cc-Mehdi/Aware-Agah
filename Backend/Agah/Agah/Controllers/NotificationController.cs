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
                        var notificationsList = _unitOfWork.Notification_UserRepository.GetAllByFilter(u => u.UserId == userId, includeProperties: u => u.User).OrderByDescending(u=>u.CreatedAt);
                        return Ok(notificationsList);
                    default:
                        break;
                }

                return BadRequest(new { message = "امکان ارسال اعلان با روش انتخاب شده وجود ندارد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" });
            }
        }

        [HttpGet("ReadAllNotifications/{userId}")]
        public async Task<IActionResult> ReadAllNotifications(int userId)
        {
            try
            {
                // TODO : check user token with user id
                var user = _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == userId);

                if(user == null)
                    return BadRequest(new { message = "کاربر مورد نظر یافت نشد" });

                var notificationsList = _unitOfWork.Notification_UserRepository.GetAllByFilter(u => u.UserId == userId);

                if (notificationsList == null)
                    return BadRequest(new { message = "اعلانی برای کاربر مورد نظر یافت نشد" });

                foreach (var notification in notificationsList)
                {
                    notification.IsRead = true;
                    _unitOfWork.Notification_UserRepository.Update(notification);
                }

                await _unitOfWork.SaveAsync();
                return Ok(notificationsList);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" });
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
