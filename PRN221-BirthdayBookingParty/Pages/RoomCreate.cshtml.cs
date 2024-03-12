using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using System.Net.Mail;
using System.Net;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    public class RoomCreateModel : PageModel
    {
        public int Capacity { get; set; }
        public string RoomStatus { get; set; }

        public decimal RoomPrice { get; set; }

        private IRepositoryBase<Room> _roomRepository;
        public RoomCreateModel()
        {
            _roomRepository = new RoomRepository();
        }
        public IActionResult OnPost()
        {

            Room room = new Room
            {
                Capacity = Capacity,    
                RoomStatus = Request.Form["RoomStatus"],
                RoomPrice = RoomPrice
            };

            _roomRepository.Add(room);
            return RedirectToPage("/RoomManagement");
        }
    }
}
