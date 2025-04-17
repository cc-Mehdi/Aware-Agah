using Agah.Services;
using Agah.Services.Interfaces;
using Agah.Utility;
using Agah.ViewModels;
using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Agah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
                return Ok(new ViewModels.ResponseStatus { StatusCode = 401, StatusMessage = "ایمیل یا کلمه عبور اشتباه است!" });

            var token = GenerateJwtToken(user);
            return Ok(new { StatusCode = 200, StatusMessage = "ورود شما با موفقیت انجام شد", Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (_unitOfWork.UserRepository.IsUserExist(request.Email))
                return Ok(new ViewModels.ResponseStatus { StatusCode = 400, StatusMessage = "ایمیل وارد شده قبلا ثبت نام شده است" });

            var user = new User
            {
                Email = request.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User" // Default role
            };

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();

            return Ok(new ViewModels.ResponseStatus { StatusCode = 200, StatusMessage = "ثبت نام شما با موفقیت انجام شد" });
        }

        [Authorize]
        [HttpGet("ValidateToken")]
        public IActionResult ValidateToken()
        {
            return Ok(new { valid = true });
        }

        // ارسال کد تأیید ایمیل
        [Authorize]
        [HttpPost("SendEmailVerification")]
        public async Task<IActionResult> SendEmailVerification()
        {
            User user = await Auth.GetLoggedInUserAsync(HttpContext, _unitOfWork);
            if (user == null)
                return BadRequest(new ResponseStatus() { StatusCode = 200, StatusMessage = "کاربر یافت نشد." });

            var token = Auth.GenerateRandomToken(); // مثلاً: "123456"
            user.EmailVerificationToken = token;
            user.EmailVerificationTokenExpiry = DateTime.Now.AddMinutes(10);

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveAsync();
            await _emailService.SendVerificationEmailAsync(user.Email, token);

            return Ok(new ResponseStatus() { StatusCode = 200, StatusMessage = "کد تأیید به ایمیل شما ارسال شد." });
        }

        // تأیید ایمیل
        [Authorize]
        [HttpPost("VerifyEmail/{token}")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            User user = await Auth.GetLoggedInUserAsync(HttpContext, _unitOfWork);
            if (user == null)
                return Ok(new ResponseStatus() { StatusCode = 400, StatusMessage = "کاربر یافت نشد." });

            if (user.EmailVerificationToken != token || user.EmailVerificationTokenExpiry < DateTime.Now)
                return Ok(new ResponseStatus() { StatusCode = 400, StatusMessage = "کد نامعتبر یا منقضی شده است." });

            user.IsEmailVerified = true;

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveAsync();
            return Ok(new ResponseStatus() { StatusCode = 200, StatusMessage = "ایمیل با موفقیت تأیید شد." });
        }
    }
}
