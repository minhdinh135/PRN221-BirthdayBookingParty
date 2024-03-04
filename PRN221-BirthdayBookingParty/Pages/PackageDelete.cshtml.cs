using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class PackageDeleteModel : PageModel
    {
        private IRepositoryBase<Package> _packageRepository;

        [BindProperty]
        public Package package { get; set; }

        public void OnGet(int id)
        {
            _packageRepository = new PackageRepository();
            package = _packageRepository.GetAll().FirstOrDefault(p => p.PackageId == id);
        }

        public IActionResult OnPost(int id)
        {
            _packageRepository = new PackageRepository();
            package = _packageRepository.GetAll().FirstOrDefault(p => p.PackageId == id);

            _packageRepository.Delete(package);
            return RedirectToPage("/PackageManagement");
        }
    }
}
