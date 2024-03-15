using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Pages
{

    [BindProperties]
    [Authorize(Policy = "LoginSessionPolicy")]
    public class BookingListModel : PageModel
    {
		private IRepositoryBase<Booking> _bookingRepository;
        private IRepositoryBase<Payment> _paymentRepository;

        public IList<Booking> BookingsWithPayments { get; set; }
        
		public List<Booking> Bookings { get; set; }

        public List<Payment> Payments { get; set; }

        public BookingListModel()
        {
            _bookingRepository = new BookingRepository();
            _paymentRepository = new PaymentRepository();

        }

        public void OnGet()
		{
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
           
            Payments = _paymentRepository.GetAll();
        }
	}
}
