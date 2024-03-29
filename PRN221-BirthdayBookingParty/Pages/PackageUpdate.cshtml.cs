using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]

    [BindProperties]
    public class PackageUpdateModel : PageModel
    {
        public int PackageId { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Description can only contain alphabetic characters and spaces")]
        public string PackageName { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Package type must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Package type can only contain alphabetic characters and spaces")]
        public string PackageType { get; set; }

        private readonly IRepositoryBase<Package> _packageRepository;
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
