using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]

    [BindProperties]
    public class PackageCreateModel : PageModel
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Package name must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Package name can only contain alphabetic characters and spaces")]
        public string PackageName { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Package type must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Package type can only contain alphabetic characters and spaces")]
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (IsDuplicate(PackageName, PackageType))
            {
                ModelState.AddModelError(string.Empty, "Package name or type already exists.");
                return Page();
            }
            Package package = new Package
            {
                PackageName = PackageName,
                PackageType = PackageType,
            };
            
            packageRepository.Add(package);

            return RedirectToPage("/PackageManagement");
        }
        public bool IsDuplicate(string name, string type)
        {
            var existingCustomer = packageRepository.GetAll().FirstOrDefault(c => c.PackageName == name || c.PackageType == type);
            return existingCustomer != null;
        }
    }
}
