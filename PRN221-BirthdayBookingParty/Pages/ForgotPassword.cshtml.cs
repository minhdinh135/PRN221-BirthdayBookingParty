using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using System.Net.Mail;
using System.Net;
using Models;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IRepositoryBase<User> _userRepository;

        public ForgotPasswordModel(IRepositoryBase<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public string Email { get; set; }

        public IActionResult OnPost()
        {
            string from = "nguyenhoanglam18112003@gmail.com";
            string pass = "ltme yxjb lzne dcmb";

            var user = _userRepository.GetAll().FirstOrDefault(u => u.Email == Email);
            if (user != null)
            {
                try
                {
                    // Sending OTP via email
                    MailMessage mail = new MailMessage();
                    mail.To.Add(Email);
                    mail.From = new MailAddress(from);
                    mail.Subject = "Birthday Party OTP Authentication Account";

                    Random random = new Random();
                    int otpCode = random.Next(100000, 999999);
                    mail.Body = "Your OTP verification code is " + otpCode;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(from, pass);

                    TempData["OTP"] = otpCode.ToString();
                    TempData["Email"] = Email;

                    smtp.Send(mail);

                    return RedirectToPage("/OTP_Form");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return Page();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email.");
                return Page();
            }
        }
    }
}
