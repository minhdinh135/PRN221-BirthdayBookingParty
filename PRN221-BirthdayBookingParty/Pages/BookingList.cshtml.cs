using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using System.Text.Json;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class BookingListModel : PageModel
    {
		private IRepositoryBase<Booking> _bookingRepository;

		public List<Booking> Bookings { get; set; }
		public void OnGet()
		{
            _bookingRepository = new BookingRepository();

            var isCustomer = HttpContext.Session.Keys.Contains("CUSTOMER");

			if(isCustomer)
			{
                var customerString = HttpContext.Session.GetString("CUSTOMER");
                var customer = JsonSerializer.Deserialize<User>(customerString);

                Bookings = _bookingRepository.GetAll().Where(b => b.UserId == customer.UserId).ToList();
            }
            else
            {
                Bookings = _bookingRepository.GetAll();
            }
        }
	}
}
