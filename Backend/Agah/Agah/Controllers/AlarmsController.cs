using Datalayer.Repositories.IRepositories;
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

        [HttpGet("GetAlarms")]
        public async Task<IActionResult> GetAlarms()
        {
            try
            {
                var list = await _unitOfWork.AlarmRepository.GetAllAsync();

                return Ok(new {result = list.Select(u => new { u.Id, u.PersianName, u.ShortDescription, u.IsActive }).ToList() });
            }
            catch (Exception ex)
            {
                return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
            }
        }

    }
}
