using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]
    public class RoomDeleteModel : PageModel
    {
        private IRepositoryBase<Room> _roomRepository;

        [BindProperty]
        public Room room { get; set; }

        public void OnGet(int id)
        {
            _roomRepository = new RoomRepository();
            room = _roomRepository.GetAll().FirstOrDefault(r => r.RoomId == id);
        }

        public IActionResult OnPost(int id)
        {
            _roomRepository = new RoomRepository();

            room = _roomRepository.GetAll().FirstOrDefault(r => r.RoomId == id);
            room.RoomStatus = "Deleted";
            _roomRepository.Update(room);

            return RedirectToPage("/RoomManagement");
        }
    }
}
