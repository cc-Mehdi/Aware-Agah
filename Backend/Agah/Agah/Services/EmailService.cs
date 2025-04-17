using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Agah.Configuration;
using Agah.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Agah.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toAddress, string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                // بررسی فرمت صحیح ایمیل برای جلوگیری از حملات Email Injection
                if (!Regex.IsMatch(toAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("Invalid email address format.");

                // تجزیه مقدار body برای استخراج اطلاعات کلیدی
                string[] lines = body.Split("\n");
                string productName = lines[0].Replace("محصول :", "").Trim();
                string price = lines[1].Replace("قیمت فعلی :", "").Trim();
                string reservedRange = lines[2].Replace("بازه رزرو شده :", "").Trim();

                // قالب HTML داینامیک
                string htmlBody = $@"
        <!DOCTYPE html>
        <html lang='fa'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>{subject}</title>
            <style>
                body {{ font-family: Arial, sans-serif; direction: rtl; text-align: right; background-color: #f8f9fa; margin: 0; padding: 0; }}
                .email-container {{ max-width: 600px; margin: 20px auto; background: #fff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                .header {{ text-align: center; padding: 10px 0; }}
                .header img {{ max-width: 150px; }}
                .content {{ font-size: 16px; line-height: 1.8; color: #333; }}
                .highlight {{ font-size: 18px; font-weight: bold; color: #d9534f; }}
                .button {{ display: block; width: 200px; margin: 20px auto; padding: 10px; background: #007bff; color: #fff; text-align: center; text-decoration: none; border-radius: 5px; font-size: 16px; }}
                .footer {{ text-align: center; font-size: 14px; color: #666; margin-top: 20px; }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>
                    <img src='http://localhost:5173/src/assets/images/logos/Full%20color.png' alt='لوگوی برند'>
                </div>
                <div class='content'>
                    <p>کاربر گرامی،</p>
                    <p>قیمت <strong>{productName}</strong> به‌روزرسانی شد!</p>
                    <p class='highlight'>قیمت فعلی: {price} ریال</p>
                    <p>بازه رزرو شده: {reservedRange}</p>
                    <p>برای مشاهده جزئیات بیشتر، روی دکمه زیر کلیک کنید:</p>
                    <a href='http://localhost:5173/' class='button'>مشاهده قیمت‌ها</a>
                </div>
                <div class='footer'>
                    <p>این ایمیل به صورت خودکار ارسال شده است، لطفاً به آن پاسخ ندهید.</p>
                    <p>© ۲۰۲۵ آگاه - Agah | تمامی حقوق محفوظ است.</p>
                </div>
            </div>
        </body>
        </html>";

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Agah | آگاه", _emailSettings.FromAddress));
                email.To.Add(new MailboxAddress("", toAddress));
                email.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = isBodyHtml ? htmlBody : null,
                    TextBody = isBodyHtml ? null : body
                };

                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_emailSettings.User, _emailSettings.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

        public async Task SendVerificationEmailAsync(string toAddress, string token)
        {
            try
            {
                // بررسی فرمت صحیح ایمیل برای جلوگیری از حملات Email Injection
                if (!Regex.IsMatch(toAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("Invalid email address format.");

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Agah | آگاه", _emailSettings.FromAddress));
                email.To.Add(new MailboxAddress("", toAddress));
                email.Subject = "فعال سازی حساب آگاه | Agah";

                // قالب HTML داینامیک
                string htmlBody = $@"
        <!DOCTYPE html>
        <html lang='fa'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>کد تایید حساب آگاه | Agah</title>
            <style>
                body {{ font-family: Arial, sans-serif; direction: rtl; text-align: right; background-color: #f8f9fa; margin: 0; padding: 0; }}
                .email-container {{ max-width: 600px; margin: 20px auto; background: #fff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                .header {{ text-align: center; padding: 10px 0; }}
                .header img {{ max-width: 150px; }}
                .content {{ font-size: 16px; line-height: 1.8; color: #333; }}
                .highlight {{ font-size: 18px; font-weight: bold; color: #d9534f; }}
                .button {{ display: block; width: 200px; margin: 20px auto; padding: 10px; background: #007bff; color: #fff; text-align: center; text-decoration: none; border-radius: 5px; font-size: 16px; }}
                .footer {{ text-align: center; font-size: 14px; color: #666; margin-top: 20px; }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>
                    <img src='http://localhost:5173/src/assets/images/logos/Full%20color.png' alt='لوگوی برند'>
                </div>
                <div class='content'>
                    <p>کد تأیید شما: <strong>{token}</strong></p>
                    <p> برای فعال سازی حساب خود وارد تنظیمات حساب کاربری در سامانه آگاه شده و در بخش تنظیمات حساب > فعال سازی ایمیل کد مربوطه را وارد کنید </p>
                    <a href='http://localhost:5173/Profile' class='button'>مدیریت حساب</a>
                </div>
                <div class='footer'>
                    <p>این ایمیل به صورت خودکار ارسال شده است، لطفاً به آن پاسخ ندهید.</p>
                    <p>© ۲۰۲۵ آگاه - Agah | تمامی حقوق محفوظ است.</p>
                </div>
            </div>
        </body>
        </html>";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };

                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_emailSettings.User, _emailSettings.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}
