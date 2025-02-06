using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Agah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceAlertController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PriceAlertController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> SetPriceAlert([FromBody] Reserve_ViewModel bodyContent)
        {
            try
            {
                // Validation Id's
                var user = _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == bodyContent.UserId);
                if (user == null)
                    return BadRequest(new { message = "کاربر یافت نشد" });

                var product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == bodyContent.ProductId);
                if (product == null)
                    return BadRequest(new { message = "محصول یافت نشد" });

                var alarm = _unitOfWork.AlarmRepository.GetFirstOrDefault(u => u.Id == bodyContent.AlarmId);
                if (alarm == null)
                    return BadRequest(new { message = "هشدار یافت نشد" });

                // Delete old user reservation
                _unitOfWork.ReserveRepository.Remove(_unitOfWork.ReserveRepository.GetFirstOrDefault(u => u.User_Id == bodyContent.UserId));

                // Create new model and add them to database
                _unitOfWork.ReserveRepository.Add(new Reserve()
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

        [HttpGet("CheckPriceInReserveds/{userId}")]
        public async Task<IActionResult> CheckPriceInReserveds(int userId)
        {
            try
            {
                var reserve = _unitOfWork.ReserveRepository.GetFirstOrDefault(u => u.User_Id == userId);
                if (reserve == null)
                    return BadRequest(new { message = "رزرو بازه زمانی برای کاربر انتخابی یافت نشد" });

                string productName = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == reserve.Product_Id).Title;

                // get last products price
                decimal lastProductsPrice = _unitOfWork.ProductLogRepository.GetAllByInclude()
                    .Where(u=> u.Product_Id == reserve.Product_Id)
                    .OrderByDescending(u => u.CreatedAt)
                    .GroupBy(u => u.Product_Id)
                    .Select(g => g.First())
                    .Select(u => u.Price)
                    .FirstOrDefault();

                if (lastProductsPrice < reserve.MinPrice || lastProductsPrice > reserve.MaxPrice)
                    return Ok(new { result = $"قیمت محصول {productName} از محدوده ثبت شده خارج شده است.\nقیمت فعلی محصول : {lastProductsPrice.ToString("N0")}\nبازه رزرو شده : {reserve.MinPrice.ToString("N0")} - {reserve.MaxPrice.ToString("N0")}" });

                return Ok(new { result = "قیمت در بازه رزرو شده میباشد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" });
            }
        }
    }

    public class Reserve_ViewModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int AlarmId { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
