using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Pages
{
	[Authorize("LoginSessionPolicy")]
    public class RoomManagementModel : PageModel
    {
		private IRepositoryBase<Room> _roomRepository;
		public List<Room> Rooms { get; set; }
		public void OnGet()
		{
			_roomRepository = new RoomRepository();
			Rooms = _roomRepository.GetAll();
		}
	}
}
