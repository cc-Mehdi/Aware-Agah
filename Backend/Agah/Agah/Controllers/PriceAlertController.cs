using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                if(user == null)
                    return BadRequest(new { message = "کاربر یافت نشد" });

                var product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == bodyContent.ProductId);
                if (product == null)
                    return BadRequest(new { message = "محصول یافت نشد" });

                var alarm = _unitOfWork.AlarmRepository.GetFirstOrDefault(u => u.Id == bodyContent.AlarmId);
                if (alarm == null)
                    return BadRequest(new { message = "هشدار یافت نشد" });


                // Create new model and add them to database
                _unitOfWork.ReserveRepository.Add(new Reserve()
                {
                    User_Id = bodyContent.UserId,
                    User = user,
                    Product_Id = bodyContent.ProductId,
                    Product = product,
                    Alarm_Id = bodyContent.AlarmId,
                    Alarm = alarm,
                    MinPrice = bodyContent.MinPrice,
                    MaxPrice = bodyContent.MaxPrice,
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

        [HttpGet("GetProductNames")]
        public async Task<IActionResult> GetProductNames()
        {
            // Create the list
            List<string> items = new List<string>
        {
            "گرم طلای 18 عیار",
            "سکه بهار آزادی"
        };

            // Convert the list to JSON
            string json = JsonConvert.SerializeObject(items);


            return Ok(new { result = json });
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
