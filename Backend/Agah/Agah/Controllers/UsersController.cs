using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("User")]
        public async Task<IActionResult> User()
        {
            try
            {
                if (!Auth.IsUserExist(HttpContext))
                    return BadRequest("کاربر شناسایی نشد.");

                // Fetch user from the database using the email
                User user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email == Auth.GetUserEmail(HttpContext));

                if (user == null)
                {
                    return NotFound("کاربر پیدا نشد.");
                }

                return Ok(new { result = user });
            }
            catch (Exception ex)
            {
                return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
            }
        }

    }
}
