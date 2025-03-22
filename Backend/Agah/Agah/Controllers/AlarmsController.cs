using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AlarmsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("GetAlarms")]
        public async Task<IActionResult> GetAlarms()
        {
            try
            {
                var list = await _unitOfWork.AlarmRepository.GetAllAsync();

                return Ok( list.Select(u => new { u.Id, u.PersianName, u.ShortDescription, u.IsActive }).ToList() );
            }
            catch (Exception ex)
            {
                return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("SeedAlarms")]
        public async Task<IActionResult> SeedAlarms()
        {
            try
            {
                // Seed Notification 1
                await _unitOfWork.AlarmRepository.AddAsync(new Alarm()
                {
                    Id = 1,
                    PersianName = "نوتیفیکیشن",
                    AlarmPrice= 10000,
                    CreatedAt = DateTime.Parse("2025-03-10 00:00:00.0000000"),
                    ShortDescription= "اعلان از طریق نوتیفیکیشن درون برنامه",
                    EnglishName = "Alert",
                    IsActive = true
                });

                // Seed Notification 3
                await _unitOfWork.AlarmRepository.AddAsync(new Alarm()
                {
                    Id = 3,
                    PersianName = "ایمیل",
                    AlarmPrice = 200000,
                    CreatedAt = DateTime.Parse("2025-03-10 00:00:00.0000000"),
                    ShortDescription = "اطلاع رسانی با ایمیل",
                    EnglishName = "Email",
                    IsActive = false
                });

                // Seed Notification 4
                await _unitOfWork.AlarmRepository.AddAsync(new Alarm()
                {
                    Id = 4,
                    PersianName = "پیامک",
                    AlarmPrice = 300000,
                    CreatedAt = DateTime.Parse("2025-03-10 00:00:00.0000000"),
                    ShortDescription = "اطلاع رسانی با پیامک SMS",
                    EnglishName = "SMS",
                    IsActive = false
                });

                // Seed Notification 5
                await _unitOfWork.AlarmRepository.AddAsync(new Alarm()
                {
                    Id = 5,
                    PersianName = "تماس تلفنی",
                    AlarmPrice = 500000,
                    CreatedAt = DateTime.Parse("2025-03-10 00:00:00.0000000"),
                    ShortDescription = "اطلاع رسانی با تماس تلفنی",
                    EnglishName = "Call",
                    IsActive = false
                });

                _unitOfWork.SaveAsync();

                var list = await _unitOfWork.AlarmRepository.GetAllAsync();

                var res = list.Select(u => new { u.Id, u.PersianName, u.ShortDescription, u.IsActive }).ToList();

                return Ok(new { result = res });
            }
            catch (Exception ex)
            {
                return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
            }
        }

    }
}
