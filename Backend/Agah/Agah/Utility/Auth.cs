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
    }
}