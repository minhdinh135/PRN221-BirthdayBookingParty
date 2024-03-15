using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "AdminSessionPolicy")]

    public class HostCreateModel : PageModel
    {
        [BindProperty]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "FullName must be between 2 and 40 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "CustomerFullName can only contain alphabetic characters and spaces")]
        public string FullName { get; set; }
        [BindProperty]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telephone must be a valid phone number")]
        public string Telephone { get; set; }
        [BindProperty]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
        public string Password { get; set; }

        [BindProperty]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 50 characters")]
        public string Address { get; set; }
        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email format")]
        public string EmailAddress { get; set; }
        [BindProperty]
        public string RoleId { get; set; }
        [BindProperty]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get;set; } = DateTime.Now;

        private IRepositoryBase<User> _userRepository;
        public HostCreateModel()
        {
            _userRepository = new UserRepository();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            DateTime birthday = new DateTime(Birthday.Year, Birthday.Month, Birthday.Day);
            int age = DateTime.Now.Year - Birthday.Year;
            if (age < 16)
            {
                ModelState.AddModelError("Birthday", "You must be at least 16 years old.");
                return Page();
            }
            if (age > 99)
            {
                ModelState.AddModelError("Birthday", "You must not be over 100 years old.");
                return Page();
            }
            if (IsDuplicate(Telephone, EmailAddress))
            {
                ModelState.AddModelError(string.Empty, "Phone or Email already exists.");
                return Page();
            }

            User user = new User
            {
                FullName = FullName,
                Phone = Telephone,
                Password = Password,
                Email = EmailAddress,
                Address = Address,
                Birthday = Birthday,
                RoleId = 2,
                Status = 1,
            };

            _userRepository.Add(user);
            return RedirectToPage("/HostManagement");
        }
        public bool IsDuplicate(string phone, string emailAddress)
        {
            var existingCustomer = _userRepository.GetAll().FirstOrDefault(c => c.Phone == phone || c.Email == emailAddress);
            return existingCustomer != null;
        }
    }
}
