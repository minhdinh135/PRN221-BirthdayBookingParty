using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Models;
using Repositories;
using Repositories.Impl;
using Services.MomoService;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    [Authorize(Policy = "CustomerSessionPolicy")]
    public class PaymentManagementModel : PageModel
    {
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; }
        public string FormOfPayment { get; set; }
        public decimal PaidMoney { get; set; }
        public List<Service> Services { get; set; }

        private readonly BookingRepository bookingRepository;
        private readonly PaymentRepository paymentRepository;
        private readonly ServiceRepository serviceRepository;
        private readonly BookingServiceRepository bookingServiceRepository;
        private readonly RoomRepository roomRepository;
        private readonly MomoService momoService;
        public readonly MomoConfig momoConfig;

        public PaymentManagementModel(IOptions<MomoConfig> configuration)
        {
            bookingRepository = new BookingRepository();
            paymentRepository = new PaymentRepository();
            serviceRepository = new ServiceRepository();
            bookingServiceRepository = new BookingServiceRepository();
            roomRepository = new RoomRepository();
            momoService = new MomoService(configuration);
            momoConfig = configuration.Value;
        }

        public void OnGet()
        {
            string selectedServicesString = HttpContext.Session.GetString("SELECTED_SERVICES");
            Services = JsonSerializer.Deserialize<List<Service>>(selectedServicesString);

            string bookingString = HttpContext.Session.GetString("BOOKING");
            Booking booking = JsonSerializer.Deserialize<Booking>(bookingString);

            string roomString = HttpContext.Session.GetString("ROOM");
            Room room = JsonSerializer.Deserialize<Room>(roomString);

            PaymentStatus = "Not yet";
            TotalPrice = Services.Sum(s => s.Price) + room.RoomPrice;
        }

        public IActionResult OnPost()
        {
            string selectedServicesString = HttpContext.Session.GetString("SELECTED_SERVICES");
            Services = JsonSerializer.Deserialize<List<Service>>(selectedServicesString);

            string bookingString = HttpContext.Session.GetString("BOOKING");
            Booking booking = JsonSerializer.Deserialize<Booking>(bookingString);

            string roomString = HttpContext.Session.GetString("ROOM");
            Room room = JsonSerializer.Deserialize<Room>(roomString);
            room.RoomStatus = "Active";
            roomRepository.Update(room);

            TotalPrice = Services.Sum(s => s.Price) + room.RoomPrice;

            bookingRepository.Add(booking);

            foreach (Service service in Services)
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
                PaymentStatus = "Not yet",
                FormOfPayment = FormOfPayment,
                BookingId = booking.BookingId
            };

            paymentRepository.Add(payment);

            if (FormOfPayment == "Momo")
            {
                MomoOneTimePaymentRequest request = momoService.CreateRequestModel((long)TotalPrice, "Thank you", null);
                var result = request.GetLink(momoConfig.PaymentUrl);
                payment.PaidMoney = TotalPrice;
                payment.PaymentStatus = "MomoDone";
                booking.BookingStatus = "Accept";

                bookingRepository.Update(booking);
                paymentRepository.Update(payment);

                return Redirect(result.Item2);
            }
            else if (FormOfPayment == "Cash")
            {
                TempData["Message"] = "A request has been sent to the host for payment confirmation.";
                TempData.Keep("Message");
                return RedirectToPage("/BookingList");
            }
            else
            {
                TempData["Message"] = "Invalid payment method";
                return Page();
            }
        }

    }
}
