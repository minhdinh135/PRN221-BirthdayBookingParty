using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "AdminSessionPolicy")]

    public class HostDeleteModel : PageModel
    {
        private IRepositoryBase<User> _userRepository;

        [BindProperty]
        public User user { get; set; }

        public void OnGet(int id)
        {
            _userRepository = new UserRepository();
            user = _userRepository.GetAll().FirstOrDefault(u => u.UserId == id);
        }

        public IActionResult OnPost(int id)
        {
            _userRepository = new UserRepository();
            user = _userRepository.GetAll().FirstOrDefault(u => u.UserId == id);

            _userRepository.Delete(user);
            return RedirectToPage("/HostManagement");
        }
    }
}
