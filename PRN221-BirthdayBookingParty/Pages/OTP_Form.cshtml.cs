using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class OTP_FormModel : PageModel
    {
        private readonly IRepositoryBase<User> _userRepository;

        public OTP_FormModel(IRepositoryBase<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [TempData]
        public int OTPCode { get; set; }

        [BindProperty]
        public int EnteredOTP { get; set; }

        // Retrieve user after successful OTP verification
        public IActionResult OnPost()
        {
            if (EnteredOTP.ToString() == TempData["OTP"] as string)
            {
                string email = TempData["Email"] as string;
                var user = _userRepository.GetAll().FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    return RedirectToPage("/ChangePassword", new { email = user.Email});
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return Page();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid OTP. Please try again.");
                return Page();
            }
        }
    }
}
