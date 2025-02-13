using Datalayer.Models;
using Datalayer.Repositories;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

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
                var list = _unitOfWork.AlarmRepository.GetAll().Select(u=> new { u.Id, u.PersianName, u.ShortDescription, u.IsActive }).ToList();

                return Ok(new {result = list});
            }
            catch (Exception ex)
            {
                return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
            }
        }

    }
}
