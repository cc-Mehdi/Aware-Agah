using Datalayer.Models;
using Datalayer.Repositories.IRepositories;

namespace Agah.Utility
{
    public static class Auth
    {
        public static bool IsUserExist(HttpContext httpContext)
        {
            // Extract email from the JWT token (assuming it's stored in the claims)
            var userEmail = httpContext.User.Identities.First().Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return false;
            }

            return true;
        }

        public static string GetUserEmail(HttpContext httpContext)
        {
            // Extract email from the JWT token (assuming it's stored in the claims)
            var userEmail = httpContext.User?.Identities?.First()?.Name ?? "";

            return userEmail;
        }

        public static async Task<User> GetLoggedInUserAsync(HttpContext httpContext, IUnitOfWork _unitOfWork)
        {
            if (!Auth.IsUserExist(httpContext))
                return null;

            // Fetch user from the database using the email
            User user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email == Auth.GetUserEmail(httpContext));

            if (user == null)
                return null;

            return user;
        }

        public static string GenerateRandomToken()
        {
            return new Random().Next(100000, 999999).ToString(); // کد ۶ رقمی
        }
    }
}