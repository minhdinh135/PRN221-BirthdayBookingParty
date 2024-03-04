using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    public class ServiceCreateModel : PageModel
    {
        public string ServiceName { get; set; }

        public int PackageId { get; set; }

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
            if (string.IsNullOrEmpty(ServiceName) || PackageId == 0)
            {
                ModelState.AddModelError("", "Service name and package must be provided.");
                return Page();
            }

            Service service = new Service
            {
                PackageId = PackageId,
                ServiceName = ServiceName,
            };

            try
            {
                serviceRepository.Add(service);
                return RedirectToPage("/ServiceList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while adding the service: {ex.Message}");
                return Page();
            }
        }
    }
}
