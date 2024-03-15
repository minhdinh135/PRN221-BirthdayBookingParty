using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]

    public class RoomUpdateModel : PageModel
    {
        [BindProperty]
        public int RoomId { get; set; }

        [BindProperty]
        [Range(10, 100, ErrorMessage = "Capacity must be between 10 and 100.")]
        public int Capacity { get; set; }

        [BindProperty]
        public string RoomStatus { get; set; }

        [BindProperty]
        [Range(0.01, double.MaxValue, ErrorMessage = "Room price must be greater than 0.")]
        public decimal RoomPrice { get; set; }

        private IRepositoryBase<Room> _roomRepository;
        public RoomUpdateModel()
        {
            _roomRepository = new RoomRepository();
        }

        public void OnGet(int id)
        {
            Room room = _roomRepository.GetAll().FirstOrDefault(r => r.RoomId == id);
            if (room != null)
            {
                RoomId = room.RoomId;
                Capacity = room.Capacity;
                RoomStatus = room.RoomStatus;
                RoomPrice = room.RoomPrice;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Room roomToUpdate = _roomRepository.GetAll().FirstOrDefault(r => r.RoomId == RoomId);
            {
                roomToUpdate.Capacity = Capacity;
                roomToUpdate.RoomStatus = RoomStatus;
                roomToUpdate.RoomPrice = RoomPrice;
            };

            _roomRepository.Update(roomToUpdate);

            return RedirectToPage("/RoomManagement");
        }
    }
}
