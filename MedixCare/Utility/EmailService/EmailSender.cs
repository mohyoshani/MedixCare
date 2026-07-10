using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace MedixCare.Utility.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtp = _configuration.GetSection("Smtp");
            var host = smtp["Host"] ?? "smtp.gmail.com";
            var port = int.TryParse(smtp["Port"], out var p) ? p : 587;
            var user = smtp["User"];
            var pass = smtp["Password"];
            var from = smtp["From"] ?? user;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                throw new InvalidOperationException("SMTP credentials are not configured. Set Smtp:User and Smtp:Password in configuration.");
            }

            var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, pass)
            };

            var message = new MailMessage(from!, email, subject, htmlMessage) { IsBodyHtml = true };

            return client.SendMailAsync(message);
        }

     
    }
}
