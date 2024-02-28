using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class BookingListModel : PageModel
    {
		private IRepositoryBase<Booking> _bookingRepository;
		public List<Booking> Bookings { get; set; }
		public void OnGet()
		{
			_bookingRepository = new BookingRepository();
			Bookings = _bookingRepository.GetAll();
		}
	}
}
