using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class ServiceListModel : PageModel
    {
        private IRepositoryBase<Service> serviceRepository;

        public List<Service> Services { get; set; }

        public void OnGet()
        {
            serviceRepository = new ServiceRepository();
            Services = serviceRepository.GetAll();
        }
    }
}
