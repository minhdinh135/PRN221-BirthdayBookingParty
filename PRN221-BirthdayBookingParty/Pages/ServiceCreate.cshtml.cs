using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.ComponentModel.DataAnnotations;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]

    [BindProperties]
    public class ServiceCreateModel : PageModel
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Service name must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Service name can only contain alphabetic characters and spaces")]
        public string ServiceName { get; set; }

        public int PackageId { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Service price must be greater than 0.")]
        public decimal Price { get; set; }
        public List<Package> Packages { get; set; } = new List<Package>();

        private ServiceRepository serviceRepository;
        private PackageRepository packageRepository;

        public ServiceCreateModel(ServiceRepository serviceRepository, PackageRepository packageRepository)
        {
            this.serviceRepository = serviceRepository;
            this.packageRepository = packageRepository;
        }

        public void OnGet()
        {
            Packages = packageRepository.GetAll();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (IsDuplicate(ServiceName))
            {
                ModelState.AddModelError(string.Empty, "Service name already exists.");
                return Page();
            }

            Service service = new Service
            {
                PackageId = PackageId,
                ServiceName = ServiceName,
                ServiceStatus = "Active",
                Price = Price,
            };

            try
            {
                serviceRepository.Add(service);
                return RedirectToPage("/ServiceManagement");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while adding the service: {ex.Message}");
                return Page();
            }
        }
        public bool IsDuplicate(string serviceName)
        {
            var existingCustomer = serviceRepository.GetAll().FirstOrDefault(c => c.ServiceName == serviceName);
            return existingCustomer != null;
        }
    }
}
