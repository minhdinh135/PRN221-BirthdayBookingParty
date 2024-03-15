using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "FullName must be between 2 and 40 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "CustomerFullName can only contain alphabetic characters and spaces")]
        public string FullName { get; set; }
        [BindProperty]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telephone must be a valid phone number")]
        public string Phone { get; set; }
        [BindProperty]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 50 characters")]
        public string Address { get; set; }
        [BindProperty]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        [BindProperty]
        public byte? Status { get; set; }
        [BindProperty]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }
        [BindProperty]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
        public string Password { get; set; }

        private readonly IRepositoryBase<User> userRepository;

        public RegisterModel(IRepositoryBase<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public void OnGet()
        {
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
			if (IsDuplicate(Phone, Email))
            {
                ModelState.AddModelError(string.Empty, "Phone or Email already exists.");
                return Page();
            }

            User user = new User


            {
                FullName = FullName,
                Email = Email,
                Phone = Phone,
                Address = Address,
                Birthday = DateTime.Parse(Birthday.ToString()),
                Status = 1,
                Password = Password,
                RoleId = 3
            };

            userRepository.Add(user);

            return RedirectToPage("/Login");
        }

        public bool IsDuplicate(string phone, string emailAddress)
        {
            var existingCustomer = userRepository.GetAll().FirstOrDefault(c => c.Phone == phone || c.Email == emailAddress);
            return existingCustomer != null;
        }
    }
}