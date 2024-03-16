using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class ChangePasswordModel : PageModel
    {

        private readonly IRepositoryBase<User> _userRepository;

        public ChangePasswordModel(IRepositoryBase<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please enter a new password.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        public string NewPassword { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public void OnGet(string email)
        {
            Email = email;
        }

        public IActionResult OnPost(string newPassword, string confirmPassword)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Email == Email);

            if (user != null)
            {
                if (newPassword.Equals(confirmPassword))
                {
                    user.Password = newPassword;
                    _userRepository.Update(user);
                    return RedirectToPage("/Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    return Page();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page();
            }
        }
    }
}
