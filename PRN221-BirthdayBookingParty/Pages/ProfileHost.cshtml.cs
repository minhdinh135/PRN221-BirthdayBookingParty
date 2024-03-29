using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.Text.Json;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]

    public class ProfileHostModel : PageModel
    {
        public User User { get; set; }
        public void OnGet()
        {
            string hostJsonString = HttpContext.Session.GetString("HOST");
            if (!string.IsNullOrEmpty(hostJsonString))
            {
                User = JsonSerializer.Deserialize<User>(hostJsonString);
            }
        }
    }
}
