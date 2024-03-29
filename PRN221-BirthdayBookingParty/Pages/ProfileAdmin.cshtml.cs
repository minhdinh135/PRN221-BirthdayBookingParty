using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.Text.Json;

namespace PRN221_BirthdayBookingParty.Pages
{
	[Authorize(Policy = "AdminSessionPolicy")]

	public class ProfileAdminModel : PageModel
	{
		public User User { get; set; }
		public void OnGet()
		{
			string adminJsonString = HttpContext.Session.GetString("ADMIN");
			if (!string.IsNullOrEmpty(adminJsonString))
			{
				User = JsonSerializer.Deserialize<User>(adminJsonString);
			}
		}
	}
}