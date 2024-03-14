using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    public class BookingListDetailsModel : PageModel
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime PartyDateTime { get; set; }
        public string BookingStatus { get; set; }
        
        public string Feedback { get; set; }
        public List<Package> Packages { get; set; } = new List<Package>();
        public int RoomId { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Service> Services { get; set; } = new List<Service>();
        public List<Service> SelectedServices { get; set; } = new List<Service>();

        private PaymentRepository paymentRepository;
        private BookingRepository bookingRepository;
        private RoomRepository roomRepository;
        private ServiceRepository serviceRepository;
        private PackageRepository packageRepository;
        private BookingServiceRepository bookingServiceRepository;

        public BookingListDetailsModel()
        {
            paymentRepository = new PaymentRepository();
            bookingRepository = new BookingRepository();
            roomRepository = new RoomRepository();
            serviceRepository = new ServiceRepository();
            packageRepository = new PackageRepository();
            bookingServiceRepository = new BookingServiceRepository();
        }

        public void OnGet(int id)
        {
            Booking booking = bookingRepository.GetAll().FirstOrDefault(b => b.BookingId == id);

            if (booking != null)
            {
                BookingId = booking.BookingId;
                PartyDateTime = booking.PartyDateTime;
                BookingStatus = booking.BookingStatus;
                BookingDate = booking.BookingDate;
                RoomId = booking.RoomId;
                Feedback = booking.Feedback;
            }

            Packages = packageRepository.GetAll();
            Rooms = roomRepository.GetAll();
            Services = serviceRepository.GetAll();

            if (booking != null)
            {
                SelectedServices = bookingServiceRepository
                    .GetAll()
                    .Where(bs => bs.BookingId == booking.BookingId)
                    .Select(bs => bs.Service)
                    .ToList();
            }
        }

        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
