using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Services.Interfaces;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]
    public class ServiceDeleteModel : PageModel
    {
        private readonly IServiceService _serviceService;
        private ServiceRepository serviceRepository;
        public ServiceDeleteModel(IServiceService serviceBookingService)
        {
            _serviceService = serviceBookingService;
            serviceRepository = new ServiceRepository();
        }

        [BindProperty]
        public Service Service { get; set; }

        public IActionResult OnGet(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service = _serviceService.GetAllService().FirstOrDefault(p => p.ServiceId == id);

            if (Service == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var existingService = serviceRepository.GetAll().FirstOrDefault(s => s.ServiceId == id);
                existingService.ServiceStatus = "Deleted";

                serviceRepository.Update(existingService);

                TempData["Message"] = $"Service '{existingService.ServiceName}' deleted successfully";
                return RedirectToPage("/ServiceManagement");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
