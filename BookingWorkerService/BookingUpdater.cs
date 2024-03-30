using Microsoft.Extensions.Logging;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWorkerService
{
    public class BookingUpdater : BackgroundService
    {
        private BookingRepository bookingRepository;
        private RoomRepository roomRepository;
        private readonly ILogger<BookingUpdater> _logger;

        public BookingUpdater(ILogger<BookingUpdater> logger)
        {
            bookingRepository = new BookingRepository();
            roomRepository = new RoomRepository();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Room status update background service is running.");

                try
                {
                    DateTime currentTime = DateTime.Now;
                    var expiredBookings = bookingRepository.GetAll().Where(b => b.PartyEndTime <= currentTime);

                    foreach (var booking in expiredBookings)
                    {
                        booking.BookingStatus = "Done";

                        bookingRepository.Update(booking);

                        var roomToUpdate = roomRepository.GetAll().FirstOrDefault(r => r.RoomId == booking.RoomId);
                        roomToUpdate.RoomStatus = "Inactive";
                        roomRepository.Update(roomToUpdate);

                        _logger.LogInformation($"Booking status updated for booking ID {booking.BookingId}.");
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred while updating room status: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
