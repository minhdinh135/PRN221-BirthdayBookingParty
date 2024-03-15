using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace PRN221_BirthdayBookingParty.Pages
{

    [BindProperties]
    [Authorize(Policy = "LoginSessionPolicy")]
    public class BookingListDetailsModel : PageModel
    {
        public List<Booking> Bookings { get; set; }
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime PartyDateTime { get; set; }
        public string BookingStatus { get; set; }
        public string Feedback { get; set; }
        public List<Package> Packages { get; set; } = new List<Package>();
        public int RoomId { get; set; }
        public string PaymentStatus { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
        public List<Service> SelectedServices { get; set; } = new List<Service>();
        public List<int> SelectedServiceIds { get; set; }

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
                PaymentStatus = paymentRepository.GetAll().FirstOrDefault(p => p.BookingId == booking.BookingId).PaymentStatus;
            }

            Packages = packageRepository.GetAll();
            Rooms = roomRepository.GetAll();
            Services = serviceRepository.GetAll();

            if (booking != null)
            {
                List<int> SelectedServicesIds = bookingServiceRepository
                    .GetAll()
                    .Where(bs => bs.BookingId == booking.BookingId)
                    .Select(bs => bs.ServiceId)
                    .ToList();

                SelectedServices = serviceRepository.GetAll().Where(s => SelectedServicesIds.Contains(s.ServiceId)).ToList();
            }

        }

        public IActionResult OnPost()
        {
            Booking bookingToUpdate = bookingRepository.GetAll().FirstOrDefault(b => b.BookingId == BookingId);
            if(bookingToUpdate == null)
            {
                return NotFound();
            }
            bookingToUpdate.PartyDateTime = PartyDateTime;
            bookingToUpdate.BookingStatus = "Pending";
            bookingToUpdate.BookingDate = BookingDate;
            bookingToUpdate.RoomId = RoomId;
            bookingToUpdate.Feedback = "N/A ";
            PaymentStatus = paymentRepository.GetAll().FirstOrDefault(p => p.BookingId == bookingToUpdate.BookingId).PaymentStatus;

            string bookingString = JsonSerializer.Serialize(bookingToUpdate);
            HttpContext.Session.SetString("BOOKING", bookingString);

            List<Service> selectedServices = serviceRepository.GetAll().Where(s => SelectedServiceIds.Contains(s.ServiceId)).ToList();
            string selectedServicesString = JsonSerializer.Serialize(selectedServices);
            HttpContext.Session.SetString("SELECTED_SERVICES", selectedServicesString);

            Room room = roomRepository.GetAll().FirstOrDefault(r => r.RoomId == RoomId);
            string roomString = JsonSerializer.Serialize(room);
            HttpContext.Session.SetString("ROOM", roomString);

            return RedirectToPage("/PaymentUpdate");
        }
    }
}
