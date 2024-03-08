using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;
using System.Net.Mail;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class BookingUpdateModel : PageModel
    {
        [BindProperty]
        public int BookingId { get; set; }
        [BindProperty]
        public string BookingStatus { get; set; }

        public IRepositoryBase<Booking> _bookingRepository;
        public List<Booking> Bookings { get; set; } = new List<Booking>();

        public BookingUpdateModel()
        {
            _bookingRepository = new BookingRepository();
        }

        public void OnGet(int id)
        {
            Booking booking = _bookingRepository.GetAll().FirstOrDefault(b => b.BookingId == id);
            if (booking != null)
            {
                BookingId = booking.BookingId;
                BookingStatus = booking.BookingStatus;
            }
        }

        public IActionResult OnPost()
        {
            Booking bookingToUpdate = _bookingRepository.GetAll().FirstOrDefault(u => u.BookingId == BookingId);
            if (bookingToUpdate == null)
            {
                return NotFound();
            }

            bookingToUpdate.BookingId = BookingId;
            bookingToUpdate.BookingStatus = BookingStatus;

            _bookingRepository.Update(bookingToUpdate);

            return RedirectToPage("/BookingList");
        }
    }
}
