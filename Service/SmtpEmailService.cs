using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SmtpEmailService: IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public SmtpEmailService(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient(configuration["Smtp:Host"], int.Parse(configuration["Smtp:Port"]))
            {
                Credentials = new NetworkCredential(configuration["Smtp:Username"], configuration["Smtp:Password"]),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress("your-email@example.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }
    }
}
