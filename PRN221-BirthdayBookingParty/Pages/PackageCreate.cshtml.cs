using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]

    [BindProperties]
    public class PackageCreateModel : PageModel
    {
        public string PackageName { get; set; }

        public string PackageType { get; set; }

        private PackageRepository packageRepository;

        public PackageCreateModel()
        {
            packageRepository = new PackageRepository();
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            Package package = new Package
            {
                PackageName = PackageName,
                PackageType = PackageType,
            };
            
            packageRepository.Add(package);

            return RedirectToPage("/PackageManagement");
        }
    }
}
