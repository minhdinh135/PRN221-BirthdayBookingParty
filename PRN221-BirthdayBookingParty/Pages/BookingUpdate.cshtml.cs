using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize("LoginSessionPolicy")]
    public class BookingUpdateModel : PageModel
    {
        [BindProperty]
        public int BookingId { get; set; }
        [BindProperty]
        public string BookingStatus { get; set; }
        [BindProperty]
        public string PaymentStatus { get; set; }
        [BindProperty]
        public string Feedback {  get; set; }
        [BindProperty]
        public int PaymentId {  get; set; }


        public IRepositoryBase<Booking> _bookingRepository;
        private IRepositoryBase<Payment> _paymentRepository;
        private IRepositoryBase<Room> _roomRepository;

        public List<Booking> Bookings { get; set; } = new List<Booking>();

        public BookingUpdateModel()
        {
            _bookingRepository = new BookingRepository();
            _paymentRepository = new PaymentRepository();
            _roomRepository = new RoomRepository();
        }

        public void OnGet(int id)
        {
            Booking booking = _bookingRepository.GetAll().FirstOrDefault(b => b.BookingId == id);
            Payment payment = _paymentRepository.GetAll().FirstOrDefault(p => p.BookingId == id);
            if (booking != null)
            {
                BookingId = booking.BookingId;
                PaymentId = payment.PaymentId;
                Feedback = booking.Feedback;
                BookingStatus = booking.BookingStatus;
                PaymentStatus = payment.PaymentStatus;
            }
        }

        public IActionResult OnPost()
        {
            Booking bookingToUpdate = _bookingRepository.GetAll().FirstOrDefault(b => b.BookingId == BookingId);
            Payment paymentToUpdate = _paymentRepository.GetAll().FirstOrDefault(p => p.BookingId == BookingId);
            Room roomToUpdate = _roomRepository.GetAll().FirstOrDefault(r => r.RoomId == bookingToUpdate.RoomId);

            if (bookingToUpdate == null)
            {
                return NotFound();
            }

            bookingToUpdate.Feedback = Feedback;
            bookingToUpdate.BookingStatus = BookingStatus;
            paymentToUpdate.PaymentStatus = PaymentStatus;

            if (BookingStatus == "Deny" || BookingStatus == "Done")
            {
                roomToUpdate.RoomStatus = "Inactive";
            }
            else if (PaymentStatus == "CashDone")
            {
                paymentToUpdate.PaidMoney = paymentToUpdate.TotalPrice;
            }

            _bookingRepository.Update(bookingToUpdate);
            _paymentRepository.Update(paymentToUpdate);
            if (roomToUpdate != null)
            {
                _roomRepository.Update(roomToUpdate);
            }

            return RedirectToPage("/BookingList");
        }

    }
}
