using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class RoomUpdateModel : PageModel
    {
        [BindProperty]
        public int RoomId { get; set; }

        [BindProperty]
        public int Capacity { get; set; }

        [BindProperty]
        public string RoomStatus { get; set; }

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
            };

            _roomRepository.Update(roomToUpdate);

            return RedirectToPage("/RoomManagement");
        }
    }
}
