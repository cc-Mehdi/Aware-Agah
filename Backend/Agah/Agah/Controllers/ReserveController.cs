using Agah.Services;
using Agah.Services.Interfaces;
using Agah.Utility;
using Agah.ViewModels;
using Azure.Messaging;
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
        private readonly IEmailService _emailService;
        private readonly ReserveService _reserveService;


        public ReserveController(IUnitOfWork unitOfWork, IEmailService emailService, ReserveService reserveService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _reserveService = reserveService;
        }

        [Authorize]
        [HttpPost("SetPriceRangeReservation")]
        public async Task<IActionResult> SetPriceRangeReservation([FromBody] Reserve_ViewModel bodyContent)
        {
            try
            {
                if (!Auth.IsUserExist(HttpContext))
                    return BadRequest("کاربر شناسایی نشد.");

                // Validation Id's
                var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email == Auth.GetUserEmail(HttpContext));
                if (user == null)
                    return BadRequest(new ResponseStatus { StatusCode = 400, StatusMessage = "کاربر یافت نشد" });

                var product = await _unitOfWork.ProductRepository.GetFirstOrDefaultAsync(u => u.Id == bodyContent.ProductId);
                if (product == null)
                    return BadRequest(new ResponseStatus { StatusCode = 400, StatusMessage = "محصول یافت نشد" });

                var alarm = await _unitOfWork.AlarmRepository.GetFirstOrDefaultAsync(u => u.Id == bodyContent.AlarmId);
                if (alarm == null)
                    return BadRequest(new ResponseStatus { StatusCode = 400, StatusMessage = "هشدار یافت نشد" });

                if (alarm.EnglishName == "Email" && user.IsEmailVerified != true)
                    return BadRequest(new ResponseStatus { StatusCode = 400, StatusMessage = "لطفا نسبت به فعال سازی ایمیل خود اقدام کنید" });

                if ((alarm.EnglishName == "Phone" || alarm.EnglishName == "SMS") && user.IsPhoneVerivied != true)
                    return BadRequest(new ResponseStatus { StatusCode = 400, StatusMessage = "لطفا نسبت به فعال سازی شماره تلفن خود اقدام کنید" });


                // Delete old user reservation
                var oldReservesList = await _unitOfWork.ReserveRepository.GetFirstOrDefaultAsync(u => u.User_Id == user.Id);
                if (oldReservesList != null)
                    _unitOfWork.ReserveRepository.Remove(oldReservesList);

                // Create new model and add them to database
                await _unitOfWork.ReserveRepository.AddAsync(new Reserve()
                {
                    User_Id = user.Id,
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
                return Ok(new ResponseStatus { StatusCode = 200, StatusMessage = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseStatus { StatusCode = 400, StatusMessage = $"عملیات با خطا مواجه شد\nخطا : {ex.Message}" });
            }
        }

        [HttpGet("CheckPriceInReserveds/{userId}")]
        public async Task<IActionResult> CheckPriceInReserveds(int userId)
        {
            var response = await _reserveService.CheckPriceInReserveds(userId);
            if (response.StatusCode == 200)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpGet("GetReserves")]
        public async Task<IActionResult> GetReserves()
        {
            var response = await _reserveService.GetReserves();
            if (response != null)
                return Ok(response);
            else
                return BadRequest("داده ای یافت نشد");
        }

        [Authorize]
        [HttpGet("Reserve")]
        public async Task<IActionResult> GetReserve()
        {
            if (!Auth.IsUserExist(HttpContext))
                return BadRequest("کاربر شناسایی نشد.");

            // Fetch user from the database using the email
            User user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email == Auth.GetUserEmail(HttpContext));

            if (user == null)
                return NotFound("کاربر پیدا نشد.");


            var reservesList = await _unitOfWork.ReserveRepository.GetAllByFilterAsync(u => u.IsSent != true, u => u.User);
            var reserve = reservesList.FirstOrDefault(u => u.User.Email == user.Email);

            if (reserve == null)
                return BadRequest("بازه رزرو شده فعالی یافت نشد");

            var result = await _unitOfWork.ReserveRepository.GetFirstOrDefaultAsync(u => u.Id == reserve.Id, u => u.User, u => u.Alarm, u => u.Product);

            return Ok(result);
        }
    }

    public class Reserve_ViewModel
    {
        public int ProductId { get; set; }
        public int AlarmId { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
    }
}
