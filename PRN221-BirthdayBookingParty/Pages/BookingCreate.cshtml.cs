using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public List<Service> SelectedServices { get; set; } = new List<Service>();

        private BookingRepository bookingRepository;
        private RoomRepository roomRepository;
        private ServiceRepository serviceRepository;
        private PackageRepository packageRepository;
        private BookingServiceRepository bookingServiceRepository;

        public BookingCreateModel()
        {
            bookingRepository = new BookingRepository();
            roomRepository = new RoomRepository();
            serviceRepository = new ServiceRepository();
            packageRepository = new PackageRepository();
            bookingServiceRepository = new BookingServiceRepository();
        }
        public void OnGet()
        {
            Packages = packageRepository.GetAll();
            Rooms = roomRepository.GetAll();
            Services = serviceRepository.GetAll();
            BookingDate = DateTime.Now;
            Status = "Pending";
            
        }

        public IActionResult OnPost(int[] serviceId1, int[] serviceId2, int[] serviceId3)
        {
            string userString = HttpContext.Session.GetString("CUSTOMER");
            var user = JsonSerializer.Deserialize<User>(userString);
            int userId = user.UserId;

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

            foreach (Service service in SelectedServices)
            {
                BookingService bookingService = new BookingService
                {
                    BookingId = booking.BookingId,
                    ServiceId = service.ServiceId,
                };

                bookingServiceRepository.Add(bookingService);
            }

            AddSelectedServices(serviceId1, booking.BookingId);
            AddSelectedServices(serviceId2, booking.BookingId);
            AddSelectedServices(serviceId3, booking.BookingId);

            return RedirectToPage("/BookingList");
        }

        private void AddSelectedServices(int[] serviceIds, int bookingId)
        {
            if (serviceIds != null)
            {
                foreach (int serviceId in serviceIds)
                {
                    BookingService bookingService = new BookingService
                    {
                        BookingId = bookingId,
                        ServiceId = serviceId,
                    };

                    bookingServiceRepository.Add(bookingService);
                }
            }
        }


    }
    
}
