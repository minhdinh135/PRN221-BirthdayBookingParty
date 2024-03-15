using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]

    [BindProperties]
    public class ServiceUpdateModel : PageModel
    {
        public int ServiceId { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Service name must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Service name can only contain alphabetic characters and spaces")]
        public string ServiceName { get; set; }

        public int PackageId { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Service price must be greater than 0.")]
        public decimal Price { get; set; }

        public List<Package> Packages { get; set; } = new List<Package>();
        private IRepositoryBase<Service> _serviceRepository;
        private IRepositoryBase<Package> _packageRepository;

        public ServiceUpdateModel()
        {
            _serviceRepository = new ServiceRepository();
            _packageRepository = new PackageRepository();
        }

        public void OnGet(int id)
        {
            Packages = _packageRepository.GetAll();
            Service service = _serviceRepository.GetAll().FirstOrDefault(s => s.ServiceId == id);
            if (service != null)
            {
                PackageId = service.PackageId;
                ServiceId = service.ServiceId;
                ServiceName = service.ServiceName;
                Price = service.Price;
            }
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
            Service serviceToUpdate = _serviceRepository.GetAll().FirstOrDefault(s => s.ServiceId == ServiceId);


            serviceToUpdate.ServiceId = ServiceId;
            serviceToUpdate.ServiceName = ServiceName;
            serviceToUpdate.PackageId = PackageId;
            serviceToUpdate.Price = Price;

            _serviceRepository.Update(serviceToUpdate);

            return RedirectToPage("/ServiceManagement");
        }
        public bool IsDuplicate(string serviceName)
        {
            var existingCustomer = _serviceRepository.GetAll().FirstOrDefault(c => c.ServiceName == serviceName);
            return existingCustomer != null;
        }
    }
}
