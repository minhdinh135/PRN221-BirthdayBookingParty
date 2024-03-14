using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "AdminSessionPolicy")]

    public class HostManagementModel : PageModel
    {
		private IRepositoryBase<User> _userRepository;
		public List<User> Users { get; set; }
		public void OnGet()
		{
			_userRepository = new UserRepository();
			Users = _userRepository.GetAll().Where(u => u.RoleId == 2).ToList();
		}
	}
}
