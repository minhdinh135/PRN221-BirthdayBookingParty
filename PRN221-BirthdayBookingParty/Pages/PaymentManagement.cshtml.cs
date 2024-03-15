using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;
using System.Text.Json;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
	[Authorize(Policy = "CustomerSessionPolicy")]
    public class PaymentManagementModel : PageModel
    {
        public decimal DepositMoney { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; }
        public List<Service> Services { get; set; }
        
        private BookingRepository bookingRepository;
        private PaymentRepository paymentRepository;
        private ServiceRepository serviceRepository;
		private BookingServiceRepository bookingServiceRepository;
		private RoomRepository roomRepository;

        public PaymentManagementModel()
        {
            bookingRepository = new BookingRepository();
            paymentRepository = new PaymentRepository();
            serviceRepository = new ServiceRepository();
			bookingServiceRepository = new BookingServiceRepository();
			roomRepository = new RoomRepository();
        }

        public void OnGet()
        {
			string selectedServicesString = HttpContext.Session.GetString("SELECTED_SERVICES");
			List<Service> selectedServices = JsonSerializer.Deserialize<List<Service>>(selectedServicesString);

			string bookingString = HttpContext.Session.GetString("BOOKING");
			Booking booking = JsonSerializer.Deserialize<Booking>(bookingString);

			string roomString = HttpContext.Session.GetString("ROOM");
			Room room = JsonSerializer.Deserialize<Room>(roomString);

			Services = selectedServices;
            PaymentStatus = "Not yet";
            TotalPrice = selectedServices.Sum(s => s.Price) + room.RoomPrice;
			DepositMoney = Decimal.Multiply((decimal)0.2, TotalPrice);
		}

        public IActionResult OnPost()
        {
			string selectedServicesString = HttpContext.Session.GetString("SELECTED_SERVICES");
			List<Service> selectedServices = JsonSerializer.Deserialize<List<Service>>(selectedServicesString);

			string bookingString = HttpContext.Session.GetString("BOOKING");
            Booking booking = JsonSerializer.Deserialize<Booking>(bookingString);

			string roomString = HttpContext.Session.GetString("ROOM");
			Room room = JsonSerializer.Deserialize<Room>(roomString);
			room.RoomStatus = "Active";
			roomRepository.Update(room);

            Services = selectedServices;
            PaymentStatus = "Not yet";
            TotalPrice = selectedServices.Sum(s => s.Price) + room.RoomPrice;
            DepositMoney = Decimal.Multiply((decimal)0.2, TotalPrice);

            bookingRepository.Add(booking);

			foreach (Service service in selectedServices)
			{
				BookingService bookingService = new BookingService
				{
					BookingId = booking.BookingId,
					ServiceId = service.ServiceId,
				};

				bookingServiceRepository.Add(bookingService);
			}

			Payment payment = new Payment
			{
				TotalPrice = TotalPrice,
				DepositMoney = DepositMoney,
				PaymentStatus = "Not yet",
				BookingId = booking.BookingId
			};

			paymentRepository.Add(payment);

			return RedirectToPage("/BookingList");
		}
    }
}
