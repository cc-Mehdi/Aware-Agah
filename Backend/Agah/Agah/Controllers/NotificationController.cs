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
    }

    public class Notification_MessageOptions
    {
        public string MessageSubject { get; set; }
        public string MessageContent { get; set; }
        public string NotificationType { get; set; }
        public User User { get; set; }
    }
}
