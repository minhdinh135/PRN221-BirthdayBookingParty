using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public int Status { get; set; }
        public int PackageId { get; set; }
        public int RoomId { get; set; }

        private BookingRepository bookingRepository;

        public BookingCreateModel()
        {
            bookingRepository = new BookingRepository();
        }
        public void OnGet()
        {
            BookingDate = DateTime.Now;
        }

        public IActionResult OnPost()
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

            return RedirectToPage("/BookingList");
        }
    }
}
