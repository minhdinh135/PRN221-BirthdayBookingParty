using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Models;
using Repositories;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    public class BookingCreateModel : PageModel
    {
        public DateTime BookingDate { get; set; }
        public DateTime PartyDateTime { get; set; }
        public string Status { get; set; }
        public List<Package> Packages { get; set; } = new List<Package>();
        public int RoomId { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Service> Services { get; set; } = new List<Service>();

        public List<int> SelectedServiceIds { get; set; }

        private BookingRepository bookingRepository;
        private RoomRepository roomRepository;
        private ServiceRepository serviceRepository;
        private PackageRepository packageRepository;
        private BookingServiceRepository bookingServiceRepository;
        private PaymentRepository paymentRepository;

        public BookingCreateModel()
        {
            bookingRepository = new BookingRepository();
            roomRepository = new RoomRepository();
            serviceRepository = new ServiceRepository();
            packageRepository = new PackageRepository();
            bookingServiceRepository = new BookingServiceRepository();
            paymentRepository = new PaymentRepository();
        }
        public void OnGet()
        {
            Packages = packageRepository.GetAll();
            Rooms = roomRepository.GetAll().Where(r => r.RoomStatus == "Inactive").ToList();
            Services = serviceRepository.GetAll();
            BookingDate = DateTime.Now;
            Status = "Pending";
            PartyDateTime = DateTime.Now;   
        }

        public IActionResult OnPost()
        {
            string userString = HttpContext.Session.GetString("CUSTOMER");
            var user = JsonSerializer.Deserialize<User>(userString);
            int userId = user.UserId;

            if (!IsPartyDateTimeValid(PartyDateTime))
            {
                ModelState.AddModelError("PartyDateTime", "Party date and time must be at least 2 days later than the current date and time.");
                return Page();
            }

            Booking booking = new Booking
            {
                BookingDate = DateTime.Now,
                PartyDateTime = PartyDateTime,
                BookingStatus = "Pending",
                Feedback = "N/A",
                RoomId = RoomId,
                
                UserId = userId
            };

            bookingRepository.Add(booking);

            List<Service> selectedServices = serviceRepository.GetAll().Where(s => SelectedServiceIds.Contains(s.ServiceId)).ToList();
            foreach (Service service in selectedServices)
            {
                BookingService bookingService = new BookingService
                {
                    BookingId = booking.BookingId,
                    ServiceId = service.ServiceId,
                };

                bookingServiceRepository.Add(bookingService);
            }

            Room room = roomRepository.GetAll().FirstOrDefault(r => r.RoomId == RoomId);
            room.RoomStatus = "Active";
            roomRepository.Update(room);

            Payment payment = new Payment
            {
                PaymentStatus = "Not yet",
                BookingId = booking.BookingId
            };

            paymentRepository.Add(payment);
            //string bookingString = JsonSerializer.Serialize(booking);
            //HttpContext.Session.SetString("BOOKING", bookingString);

            return RedirectToPage("/PaymentManagement");
        }

        private bool IsPartyDateTimeValid(DateTime partyDateTime)
        {
            return partyDateTime > DateTime.Now.AddDays(2);
        }

    }
    
}
