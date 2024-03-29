using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Text.Json;

namespace NguyenHoangLamRazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private IUserService userService;

        public string Email { get; set; }
        public string Password { get; set; }

        public LoginModel(IUserService userService)
        {
			this.userService = userService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string email, string password)
        {
			var user = userService.GetAccount(email, password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (user.RoleId.Equals(1)) {
                HttpContext.Session.SetString("ADMIN_NAME", user.FullName);
                HttpContext.Session.SetString("ADMIN", JsonSerializer.Serialize(user));
				HttpContext.Session.SetString("USER_ROLE", "Admin");
                return RedirectToPage("/Index");
			}

			if(user.RoleId.Equals(2))
			{
                HttpContext.Session.SetString("HOST_NAME", user.FullName);
                HttpContext.Session.SetString("HOST", JsonSerializer.Serialize(user));
                HttpContext.Session.SetString("USER_ROLE", "Host");
                return RedirectToPage("/Index");
			}

			if (user.RoleId.Equals(3))
			{
				HttpContext.Session.SetString("CUSTOMER_NAME", user.FullName);
				HttpContext.Session.SetString("CUSTOMER", JsonSerializer.Serialize(user));
                HttpContext.Session.SetString("USER_ROLE", "Customer");
                return RedirectToPage("/ProfileCustomer");
			}
			return Page();
		}
    }
}
