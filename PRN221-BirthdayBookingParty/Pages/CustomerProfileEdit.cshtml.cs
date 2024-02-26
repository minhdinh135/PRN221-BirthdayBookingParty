using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;
using Repositories.Interfaces;
using System.Text.Json;

namespace DinhTranNhatMinhRazorPages.Pages
{
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
	}
}
