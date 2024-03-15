using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "HostSessionPolicy")]

    [BindProperties]
    public class RoomCreateModel : PageModel
    {
        [Range(10, 100, ErrorMessage = "Capacity must be between 10 and 100.")]
        public int Capacity { get; set; }
        public string RoomStatus { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Room price must be greater than 0.")]
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
