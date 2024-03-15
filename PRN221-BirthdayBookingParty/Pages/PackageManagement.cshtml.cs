using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Pages
{
	[Authorize(Policy = "LoginSessionPolicy")]
    public class PackageManagementModel : PageModel
    {
		private IRepositoryBase<Package> _packageRepository;
		public List<Package> Packages { get; set; }
		public void OnGet()
		{
			_packageRepository = new PackageRepository();
			Packages = _packageRepository.GetAll();
		}
	}
}
