using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    public class ServiceUpdateModel : PageModel
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public int PackageId { get; set; }
        
        public string PackageType { get; set; }
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
            Service serviceToUpdate = _serviceRepository.GetAll().FirstOrDefault(s => s.ServiceId == ServiceId);


            serviceToUpdate.ServiceId = ServiceId;
            serviceToUpdate.ServiceName = ServiceName;
            serviceToUpdate.PackageId = PackageId;
            serviceToUpdate.Price = Price;

            _serviceRepository.Update(serviceToUpdate);

            return RedirectToPage("/ServiceManagement");
        }
    }
}
