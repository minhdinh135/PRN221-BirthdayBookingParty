using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "CustomerSessionPolicy")]

    [BindProperties]
    public class BookingDeleteModel : PageModel
    {
        private BookingRepository bookingRepository;
        private RoomRepository roomRepository;

        public int BookingId { get; set; }

        public BookingDeleteModel()
        {
            bookingRepository = new BookingRepository();
            roomRepository = new RoomRepository();
        }

        public void OnGet(int id)
        {
            BookingId = id;
        }

        public IActionResult OnPost(int id)
        {
            Booking booking = bookingRepository.GetAll().FirstOrDefault(b => b.BookingId == id);
            Room bookingRoom = roomRepository.GetAll().FirstOrDefault(r => booking.RoomId == r.RoomId);

            bookingRepository.Delete(booking);

            bookingRoom.RoomStatus = "Inactive";
            roomRepository.Update(bookingRoom);

            return RedirectToPage("/BookingList");
        }
    }
}
