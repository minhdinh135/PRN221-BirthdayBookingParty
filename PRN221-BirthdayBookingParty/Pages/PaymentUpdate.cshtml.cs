using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Text.Json;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "CustomerSessionPolicy")]
    public class PaymentUpdateModel : PageModel
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

        public PaymentUpdateModel()
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
            Room originalRoom = roomRepository.GetAll().FirstOrDefault(r => r.RoomId == booking.RoomId);
            originalRoom.RoomStatus = "Inactive";
            roomRepository.Update(originalRoom);

            Services = selectedServices;
            PaymentStatus = "Not yet";
            TotalPrice = selectedServices.Sum(s => s.Price) + room.RoomPrice;
            DepositMoney = Decimal.Multiply((decimal)0.2, TotalPrice);

            foreach(BookingService bookingService in bookingServiceRepository.GetAll())
            {
                if(bookingService.BookingId == booking.BookingId)
                {
                    bookingServiceRepository.Delete(bookingService);
                }
            }

            foreach (Service service in selectedServices)
            {
                BookingService bookingService = new BookingService
                {
                    BookingId = booking.BookingId,
                    ServiceId = service.ServiceId,
                };

                bookingServiceRepository.Add(bookingService);
            }

            Payment paymentToUpdate = paymentRepository.GetAll().FirstOrDefault(p => p.BookingId == booking.BookingId);
            paymentToUpdate.TotalPrice = TotalPrice;
            paymentToUpdate.DepositMoney = DepositMoney;
            paymentToUpdate.PaymentStatus = PaymentStatus;

            paymentRepository.Update(paymentToUpdate);

            return RedirectToPage("/BookingList");
        }
    }
}
