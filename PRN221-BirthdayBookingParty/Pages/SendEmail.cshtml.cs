using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    [Authorize("HostSessionPolicy")]
    public class SendEmailModel : PageModel
    {
        private readonly ILogger<SendEmailModel> _logger;
        private readonly IEmailService _emailService;
        public int UserId { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IRepositoryBase<User> _userRepository;
        public SendEmailModel(ILogger<SendEmailModel> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
            _userRepository = new UserRepository();
        }
        public void OnGet(int id)
        {
            UserId = id;

            User user = _userRepository.GetAll().FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                To = user.Email;
            }
        }

        public IActionResult OnPost()
        {
            User user = _userRepository.GetAll().FirstOrDefault(u => u.UserId == UserId);
            if (user != null)
            {
                To = user.Email;
            }

            string host = "smtp.gmail.com";
            int port = 587;
            bool enableSsl = true;
            bool defaultCredentials = false;

            string userName = "nguyenhoanglam18112003@gmail.com";
            string password = "ltme yxjb lzne dcmb";
            using (MailMessage mm = new MailMessage(userName, To))
            {
                mm.Subject = Subject;
                mm.Body = Body;
                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = host;
                    smtp.EnableSsl = enableSsl;
                    NetworkCredential networkCred = new NetworkCredential(userName, password);
                    smtp.UseDefaultCredentials = defaultCredentials;
                    smtp.Credentials = networkCred;
                    smtp.Port = port;
                    smtp.Send(mm);
                }


                return RedirectToPage("/BookingList");
            }
        }
    }
}
