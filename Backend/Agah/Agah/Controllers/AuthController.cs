using Datalayer.Models;
using Datalayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
                return Ok(new ViewModels.ResponseStatus { StatusCode = 401, StatusMessage = "ایمیل یا کلمه عبور اشتباه است!"});

            var token = GenerateJwtToken(user);
            return Ok(new { StatusCode = 200, StatusMessage = "ثبت نام شما با موفقیت انجام شد", Token = token });
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

        [HttpPost("register")]
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
    }
}
