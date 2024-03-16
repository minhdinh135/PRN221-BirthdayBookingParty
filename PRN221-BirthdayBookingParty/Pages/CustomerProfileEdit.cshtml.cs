using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.Json;

namespace DinhTranNhatMinhRazorPages.Pages
{
    [Authorize(Policy = "CustomerSessionPolicy")]

    public class CustomerProfileEditModel : PageModel
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
        public string Email { get; set; }

        private IRepositoryBase<User> userRepository;

        public CustomerProfileEditModel()
        {
            userRepository = new UserRepository();
        }
        public void OnGet()
        {
            string customerJsonString = HttpContext.Session.GetString("CUSTOMER");
            User user = JsonSerializer.Deserialize<User>(customerJsonString);

            if(user != null)
            {
                FullName = user.FullName;
                Phone = user.Phone;
                Address = user.Address;
                Birthday = user.Birthday;
                Email = user.Email;
            }
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
			if (IsDuplicate(Phone))
			{
				ModelState.AddModelError(string.Empty, "Phone already exists.");
				return Page();
			}

			userRepository = new UserRepository();

            var existingCustomer = userRepository.GetAll().FirstOrDefault(c => c.Email.Equals(Email));

            if (existingCustomer == null)
            {
                return Page();
            }

            existingCustomer.FullName = FullName;
            existingCustomer.Phone = Phone;
            existingCustomer.Address = Address;
            existingCustomer.Birthday = Birthday;

            userRepository.Update(existingCustomer);

            string existingCustomerJsonString = JsonSerializer.Serialize(existingCustomer);
            HttpContext.Session.SetString("CUSTOMER", existingCustomerJsonString);

			return RedirectToPage("/ProfileCustomer");
		}

		public bool IsDuplicate(string phone)
		{
			var existingCustomer = userRepository.GetAll().FirstOrDefault(c => c.Phone == phone);

            if (existingCustomer == null)
            {
                return false;
            }

			return true;
		}
	}
}
