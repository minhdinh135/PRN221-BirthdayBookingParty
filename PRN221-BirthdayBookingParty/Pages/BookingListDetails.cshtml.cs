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

        public BookingListDetailsModel()
        {
            bookingRepository = new BookingRepository();
            roomRepository = new RoomRepository();
            serviceRepository = new ServiceRepository();
            packageRepository = new PackageRepository();
            bookingServiceRepository = new BookingServiceRepository();
        }

        public void OnGet(int id)
        {
            Booking booking = bookingRepository.GetAll().FirstOrDefault(b => b.BookingId == id);
            
            if(booking != null)
            {
                BookingId = booking.BookingId;
                PartyDateTime = booking.PartyDateTime;
                Status = booking.BookingStatus;
                BookingDate = booking.BookingDate;
                RoomId = booking.RoomId;
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

        public IActionResult OnPost(int[] serviceId1, int[] serviceId2, int[] serviceId3)
        {
            Booking bookingToUpdate = bookingRepository.GetAll().FirstOrDefault(b => b.BookingId == BookingId);

            if (bookingToUpdate != null)
            {
                bookingToUpdate.PartyDateTime = PartyDateTime;
                bookingToUpdate.RoomId = RoomId;
                
            }

            List<BookingService> existingBookingServices = bookingServiceRepository.GetAll().Where(bs => bs.BookingId == bookingToUpdate.BookingId).ToList();
            
            foreach (var bookingService in existingBookingServices)
            {
                bookingServiceRepository.Delete(bookingService); 
            }

            foreach (Service service in SelectedServices)
            {
                BookingService bookingService = new BookingService
                {
                    BookingId = bookingToUpdate.BookingId,
                    ServiceId = service.ServiceId,
                };

                bookingServiceRepository.Add(bookingService);
            }

            AddSelectedServices(serviceId1, bookingToUpdate.BookingId);
            AddSelectedServices(serviceId2, bookingToUpdate.BookingId);
            AddSelectedServices(serviceId3, bookingToUpdate.BookingId);

            bookingRepository.Update(bookingToUpdate);

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
