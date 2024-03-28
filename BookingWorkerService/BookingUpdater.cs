using Microsoft.Extensions.Logging;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

                    var bookingsToUpdate = bookingRepository.GetAll().Where(b => b.PartyEndTime <= currentTime && b.BookingStatus != "Done");

                    foreach (var booking in bookingsToUpdate)
                    {
                        booking.BookingStatus = "Done";
                        bookingRepository.Update(booking);

                        var roomToUpdate = roomRepository.GetAll().FirstOrDefault(r => r.RoomId == booking.RoomId);
                        if (roomToUpdate != null)
                        {
                            roomToUpdate.RoomStatus = "Inactive";
                            roomRepository.Update(roomToUpdate);
                        }
                        else
                        {
                            _logger.LogWarning($"Room with ID {booking.RoomId} not found.");
                        }

                        _logger.LogInformation($"Booking status updated for booking ID {booking.BookingId}.");
                    }

                    var bookingsToPrepare = bookingRepository.GetAll().Where(b => b.BookingDate.AddDays(1) <= currentTime && b.BookingStatus == "Accept");

                    foreach (var booking in bookingsToPrepare)
                    {
                        booking.BookingStatus = "Preparing";
                        bookingRepository.Update(booking);

                        _logger.LogInformation($"Booking status updated to Preparing for booking ID {booking.BookingId}.");
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
