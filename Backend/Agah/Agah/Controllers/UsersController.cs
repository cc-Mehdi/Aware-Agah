using Agah.Utility;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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

        [Authorize]
        [HttpPut("User")]
        public async Task<IActionResult> User(UpdateUser_ViewModel newUser)
        {
            try
            {
                if (!Auth.IsUserExist(HttpContext))
                    return BadRequest("کاربر شناسایی نشد.");

                // Fetch user from the database using the email
                User user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email == Auth.GetUserEmail(HttpContext));

                if (user == null)
                    return NotFound("کاربر پیدا نشد.");

                if (user.Email != newUser.Email)
                    return Unauthorized("مشخصات ارسال شده همخوانی ندارد");

                user.Fullname = newUser.NewFullname;

                await _unitOfWork.SaveAsync();

                return Ok(new ResponseStatus(){StatusCode = 200, StatusMessage = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return BadRequest($"ما با خطای {ex.Message} رو به رو شده ایم 😖");
            }
        }

        public class UpdateUser_ViewModel
        {
            public string Email { get; set; }
            public string NewFullname { get; set; }
        }

    }
}
