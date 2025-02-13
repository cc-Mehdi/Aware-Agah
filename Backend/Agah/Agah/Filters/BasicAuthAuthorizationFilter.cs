using Hangfire.Annotations;
using Hangfire.Dashboard;
using System.Text;

namespace Agah.Filters
{
    public class BasicAuthAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _username;
        private readonly string _password;

        public BasicAuthAuthorizationFilter(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = httpContext.Request.Headers["Authorization"].ToString();
                var encodedCredentials = authHeader.Replace("Basic ", "");
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials)).Split(':');

                if (credentials.Length == 2)
                {
                    return credentials[0] == _username && credentials[1] == _password;
                }
            }

            httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return false;
        }
    }
}
