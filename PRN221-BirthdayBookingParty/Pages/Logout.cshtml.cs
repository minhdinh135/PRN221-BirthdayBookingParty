using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenHoangLamRazorPages.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            HttpContext.Session.Remove("Customer");
            HttpContext.Session.Remove("Admin");

            return RedirectToPage("/Login");
        }
    }
}
