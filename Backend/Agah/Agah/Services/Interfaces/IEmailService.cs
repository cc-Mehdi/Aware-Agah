namespace Agah.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toAddress, string subject, string body, bool isBodyHtml = true);
        Task SendVerificationEmailAsync(string toAddress, string token);
    }
}
