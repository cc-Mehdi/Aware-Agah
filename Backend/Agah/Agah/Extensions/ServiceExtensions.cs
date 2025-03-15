using Agah.Configuration;
using Agah.Services.Interfaces;
using Agah.Services;

namespace Agah.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailService, EmailService>();
        }
    }
}
