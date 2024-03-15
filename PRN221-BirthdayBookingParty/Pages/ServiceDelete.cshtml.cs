using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Services.Interfaces;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]
    public class ServiceDeleteModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public ServiceDeleteModel(IServiceService serviceBookingService)
        {
            _serviceService = serviceBookingService;
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
                var deletedService = _serviceService.DeleteService(id);
                TempData["Message"] = $"Service '{deletedService.ServiceName}' deleted successfully";
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
