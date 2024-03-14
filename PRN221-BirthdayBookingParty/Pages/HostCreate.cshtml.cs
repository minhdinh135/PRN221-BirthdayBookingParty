using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;
using Repositories;
using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "AdminSessionPolicy")]

    public class HostCreateModel : PageModel
    {
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string Telephone { get; set; }
        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string EmailAddress { get; set; }
        [BindProperty]
        public string RoleId { get; set; }
        [BindProperty]
        public DateTime Birthday { get;set; } = DateTime.Now;

        private IRepositoryBase<User> _userRepository;
        public HostCreateModel()
        {
            _userRepository = new UserRepository();
        }
        public IActionResult OnPost()
        {

            User user = new User
            {
                FullName = FullName,
                Phone = Telephone,
                Password = Password,
                Email = EmailAddress,
                Address = Address,
                Birthday = Birthday,
                RoleId = 2,
                Status = 1,
            };

            _userRepository.Add(user);
            return RedirectToPage("/HostManagement");
        }
    }
}
