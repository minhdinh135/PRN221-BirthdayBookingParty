using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;
using Repositories.Interfaces;
using System.Numerics;
using System.Text.Json;

namespace DinhTranNhatMinhRazorPages.Pages
{
    [Authorize(Policy = "CustomerSessionPolicy")]

    public class CustomerProfileEditModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }

        private IRepositoryBase<User> userRepository;

        public void OnGet()
        {
            string customerJsonString = HttpContext.Session.GetString("CUSTOMER");
            User = JsonSerializer.Deserialize<User>(customerJsonString);
        }

        public IActionResult OnPost()
        {
			if (!ModelState.IsValid)
			{
				return Page();
			}

			DateTime birthday = new DateTime(User.Birthday.Year, User.Birthday.Month, User.Birthday.Day);
			int age = DateTime.Now.Year - User.Birthday.Year;
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
			if (IsDuplicate(User.Phone, User.Email))
			{
				ModelState.AddModelError(string.Empty, "Phone or Email already exists.");
				return Page();
			}

			userRepository = new UserRepository();

            var existingCustomer = userRepository.GetAll().FirstOrDefault(c => c.Email.Equals(User.Email));

            if (existingCustomer == null)
            {
                return Page();
            }

            existingCustomer.FullName = User.FullName;
            existingCustomer.Phone = User.Phone;
            existingCustomer.Address = User.Address;
            existingCustomer.Birthday = User.Birthday;
            existingCustomer.Password = User.Password;

            userRepository.Update(existingCustomer);

            string existingCustomerJsonString = JsonSerializer.Serialize(existingCustomer);
            HttpContext.Session.SetString("CUSTOMER", existingCustomerJsonString);

			return RedirectToPage("/ProfileCustomer");
		}

		public bool IsDuplicate(string phone, string emailAddress)
		{
			var existingCustomer = userRepository.GetAll().FirstOrDefault(c => c.Phone == phone || c.Email == emailAddress);
			return existingCustomer != null;
		}
	}
}
