using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PRN221_BirthdayBookingParty.Pages
{
    [BindProperties]
    public class RoomCreateModel : PageModel
    {
        public int Capacity { get; set; }
        public string RoomStatus { get; set; }


        public void OnGet()
        {
        }
    }
}
