using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Service;
using Service.Interfaces;
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

        public void OnPost(string email, string password)
        {
			var user = userService.GetAccount(email, password);

            if (user == null )
            {
				Response.Redirect("/Index");
			}

            if(user.RoleId.Equals(1)) {
				HttpContext.Session.SetString("ADMIN", JsonSerializer.Serialize(user));
				Response.Redirect("/Index");
			}

			if(user.RoleId.Equals(2))
			{
				HttpContext.Session.SetString("HOST", JsonSerializer.Serialize(user));
				Response.Redirect("/Index");
			}

			if (user.RoleId.Equals(3))
			{
				HttpContext.Session.SetString("CUSTOMER", JsonSerializer.Serialize(user));
				Response.Redirect("/ProfileCustomer");
			}

			//if (admin != null && string.Equals(admin.email, email, StringComparison.OrdinalIgnoreCase) &&string.Equals(admin.password, password))
			//{
			//    HttpContext.Session.SetString("Admin", JsonSerializer.Serialize(admin));
			//    Response.Redirect("/Index");
			//}
			//else if (customer != null)
			//{
			//    HttpContext.Session.SetString("Customer", JsonSerializer.Serialize(customer));
			//    Response.Redirect("/CustomerProfile");
			//}
			//else
			//{
			//    ModelState.AddModelError(string.Empty, "Invalid email or password.");
			//}


		}
    }
}
