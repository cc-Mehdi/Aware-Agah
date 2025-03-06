using Agah.Services;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReserveController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost("SetPriceRangeReservation")]
        public async Task<IActionResult> SetPriceRangeReservation([FromBody] Reserve_ViewModel bodyContent)
        {
            try
            {
                // Validation Id's
                var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Id == bodyContent.UserId);
                if (user == null)
                    return BadRequest(new { message = "کاربر یافت نشد" });

                var product = await _unitOfWork.ProductRepository.GetFirstOrDefaultAsync(u => u.Id == bodyContent.ProductId);
                if (product == null)
                    return BadRequest(new { message = "محصول یافت نشد" });

                var alarm = await _unitOfWork.AlarmRepository.GetFirstOrDefaultAsync(u => u.Id == bodyContent.AlarmId);
                if (alarm == null)
                    return BadRequest(new { message = "هشدار یافت نشد" });

                // Delete old user reservation
                _unitOfWork.ReserveRepository.Remove(await _unitOfWork.ReserveRepository.GetFirstOrDefaultAsync(u => u.User_Id == bodyContent.UserId));

                // Create new model and add them to database
                await _unitOfWork.ReserveRepository.AddAsync(new Reserve()
                {
                    User_Id = bodyContent.UserId,
                    User = user,
                    Product_Id = bodyContent.ProductId,
                    Product = product,
                    Alarm_Id = bodyContent.AlarmId,
                    Alarm = alarm,
                    MinPrice = decimal.Parse(bodyContent.MinPrice.ToString().Replace(",", "")),
                    MaxPrice = decimal.Parse(bodyContent.MaxPrice.ToString().Replace(",", "")),
                    CreatedAt = DateTime.Now
                });

                // Save database
                await _unitOfWork.SaveAsync();

                // Return result
                return Ok(new { message = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet("CheckPriceInReserveds/{userId}")]
        public async Task<IActionResult> CheckPriceInReserveds(int userId)
        {
            try
            {
                var reserve = await _unitOfWork.ReserveRepository.GetFirstOrDefaultAsync(u => u.User_Id == userId, u=> u.User, u=> u.Product ,u=> u.Alarm);
                if (reserve == null)
                    return BadRequest(new { statusMessage = "رزرو بازه زمانی برای کاربر انتخابی یافت نشد" });

                var product = await _unitOfWork.ProductRepository.GetFirstOrDefaultAsync(u => u.Id == reserve.Product_Id);
                string productName = product?.PersianName ?? "";

                // get last products price
                var lastProductsLog = await _unitOfWork.ProductLogRepository.GetAllAsync(includeProperties: u => u.Product);
                decimal lastProductsPrice = lastProductsLog.Where(u => u.Product_Id == reserve.Product_Id)
                    .OrderByDescending(u => u.CreatedAt)
                    .GroupBy(u => u.Product_Id)
                    .Select(g => g.First())
                    .Select(u => u.Price)
                    .FirstOrDefault();


                if (lastProductsPrice < reserve.MinPrice || lastProductsPrice > reserve.MaxPrice)
                {
                    NotificationService notificationService = new NotificationService(_unitOfWork);
                    ResponseStatus response = await notificationService.SendNotification(new Notification_MessageOptions()
                    {
                        User = reserve.User,
                        NotificationType = reserve.Alarm.EnglishName,
                        MessageSubject = $"قیمت {reserve.Product.PersianName} بیش از حد تغییر کرده است!",
                        MessageContent = $"محصول : {productName}\nقیمت فعلی : {lastProductsPrice.ToString("N0")}\nبازه رزرو شده : {reserve.MinPrice.ToString("N0")} - {reserve.MaxPrice.ToString("N0")}",
                    });
                    if(response.StatusCode == 200)
                    {
                        // change status of isSent in reserve
                        reserve.IsSent = true;
                        await _unitOfWork.ReserveRepository.UpdateAsync(reserve);
                        await _unitOfWork.SaveAsync();
                        return Ok(new { statusMessage = "پیام اطلاع رسانی در صف ارسال قرار گرفت" });
                    }
                    else
                        return BadRequest(new { statusMessage = $"عملیات با خطا مواجه شد" });
                }
                return Ok(new { statusMessage = "قیمت در بازه رزرو شده میباشد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { statusMessage = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet("GetReserves")]
        public async Task<IActionResult> GetReserves()
        {
            var reservesUserIdList = await _unitOfWork.ReserveRepository.GetAllByFilterAsync(u=> u.IsSent != true);

            return Ok(reservesUserIdList);
        }
    }

    public class Reserve_ViewModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int AlarmId { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
    }
}
