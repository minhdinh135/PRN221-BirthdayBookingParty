using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;

namespace PRN221_BirthdayBookingParty.Pages.Shared
{
	[Authorize("LoginSessionPolicy")]
	public class CustomerManagementModel : PageModel
    {
		private IRepositoryBase<User> _userRepository;
		public List<User> Users { get; set; }
		public void OnGet()
        {
			_userRepository = new UserRepository();
			Users = _userRepository.GetAll().Where(u => u.RoleId == 3).ToList();
		}
    }
}
