using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.Text.Json;

namespace NguyenHoangLamRazorPages.Pages
{
    public class CustomerProfileModel : PageModel
    {
        public User User { get; set; }
        public void OnGet()
        {
            string customerJsonString = HttpContext.Session.GetString("CUSTOMER");
            if (!string.IsNullOrEmpty(customerJsonString))
            {
                User = JsonSerializer.Deserialize<User>(customerJsonString);
            }
        }
    }
}
