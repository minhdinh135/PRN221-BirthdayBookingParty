using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using System.ComponentModel.DataAnnotations;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    public class PackageUpdateModel : PageModel
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string PackageType { get; set; }

        private IRepositoryBase<Package> _packageRepository;
        public PackageUpdateModel()
        {
            _packageRepository = new PackageRepository();
        }

        public void OnGet(int id)
        {
            Package package = _packageRepository.GetAll().FirstOrDefault(p => p.PackageId == id);
            if (package != null)
            {
                PackageId = package.PackageId;
                PackageName = package.PackageName;
                PackageType = package.PackageType;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Package packageToUpdate = _packageRepository.GetAll().FirstOrDefault(p => p.PackageId == PackageId);
            if (packageToUpdate == null)
            {
                return NotFound();
            }

            packageToUpdate.PackageId = PackageId;
            packageToUpdate.PackageName = PackageName;
            packageToUpdate.PackageType = PackageType;

            _packageRepository.Update(packageToUpdate);

            return RedirectToPage("/PackageManagement");
        }
    }
}
