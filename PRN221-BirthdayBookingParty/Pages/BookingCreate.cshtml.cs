using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "CustomerSessionPolicy")]

    [BindProperties(SupportsGet = true)]
    public class BookingCreateModel : PageModel
    {
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public DateTime PartyStartTime { get; set; }
        public DateTime PartyEndTime { get; set; }
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
            PartyStartTime = DateTime.Now;
            PartyEndTime = DateTime.Now;
        }

        public IActionResult OnPost()
        {
            string userString = HttpContext.Session.GetString("CUSTOMER");
            var user = JsonSerializer.Deserialize<User>(userString);
            int userId = user.UserId;

            if (!BookingValidation.IsPartyDateTimeAfterTwoDays(PartyStartTime))
            {
                ModelState.AddModelError("PartyDateTime", "Party date and time must be at least 2 days from now.");
                return RedirectToPage();
            }

            if (!BookingValidation.IsPartyDateTimeWithinSixMonths(PartyStartTime))
            {
                ModelState.AddModelError("PartyDateTime", "Party date and time must be within 6 months from now.");
                return RedirectToPage();
            }

            if (PartyStartTime != PartyEndTime)
            {
                ModelState.AddModelError("PartyEndTime", "Party End Time and Party Start Time must be the same");
                return RedirectToPage();
            }

            if (!BookingValidation.IsPartyDateInWorkingHours(PartyStartTime))
            {
                ModelState.AddModelError("PartyDateTime", "Party date and time is not in between 6am and 0am");
                return RedirectToPage();
            }

            Booking booking = new Booking
            {
                BookingDate = DateTime.Now,
                PartyDateTime = PartyStartTime,
                PartyEndTime = PartyEndTime,
                BookingStatus = "Pending",
                Feedback = "N/A",
                RoomId = RoomId,
                
                UserId = userId
            };

			string bookingString = JsonSerializer.Serialize(booking);
			HttpContext.Session.SetString("BOOKING", bookingString);

            List<Service> selectedServices = serviceRepository.GetAll().Where(s => SelectedServiceIds.Contains(s.ServiceId)).ToList();
			string selectedServicesString = JsonSerializer.Serialize(selectedServices);
			HttpContext.Session.SetString("SELECTED_SERVICES", selectedServicesString);

            Room room = roomRepository.GetAll().FirstOrDefault(r => r.RoomId == RoomId);
			string roomString = JsonSerializer.Serialize(room);
			HttpContext.Session.SetString("ROOM", roomString);

            return RedirectToPage("/PaymentManagement");
        }



    }
    
}
