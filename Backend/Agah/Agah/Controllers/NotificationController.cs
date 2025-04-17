using Agah.Utility;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("GetNotifications/{alarmType}")]
        public async Task<IActionResult> GetNotifications(string alarmType)
        {
            try
            {
                if (!Auth.IsUserExist(HttpContext))
                    return BadRequest("کاربر شناسایی نشد.");

                // Fetch user from the database using the email
                User user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email == Auth.GetUserEmail(HttpContext));

                if (user == null)
                    return NotFound("کاربر پیدا نشد.");

                var notificationsList = await _unitOfWork.Notification_UserRepository.GetAllByFilterAsync(u => u.UserId == user.Id, includeProperties: u => u.User);
                return Ok(notificationsList.OrderByDescending(u => u.CreatedAt));

            }
            catch (Exception ex)
            {
                return BadRequest(new { statusMessage = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet("ReadAllNotifications")]
        public async Task<IActionResult> ReadAllNotifications()
        {
            try
            {
                if (!Auth.IsUserExist(HttpContext))
                    return BadRequest("کاربر شناسایی نشد.");

                // Fetch user from the database using the email
                User user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email == Auth.GetUserEmail(HttpContext));

                if (user == null)
                    return NotFound("کاربر پیدا نشد.");

                var notificationsList = await _unitOfWork.Notification_UserRepository.GetAllByFilterAsync(u => u.UserId == user.Id);

                if (notificationsList == null || !notificationsList.Any())
                    return BadRequest(new { statusMessage = "اعلانی برای کاربر مورد نظر یافت نشد" });

                foreach (var notification in notificationsList)
                {
                    notification.IsRead = true;
                    await _unitOfWork.Notification_UserRepository.UpdateAsync(notification);
                }

                await _unitOfWork.SaveAsync();

                var result = notificationsList.OrderByDescending(u => u.CreatedAt).ToList();
                return Ok(result);

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
