using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;
using System.Net;

namespace NguyenHoangLamRazorPages.Pages
{
    public class CustomerCreateModel : PageModel
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
        public DateTime Birthday { get; set; }

        private IRepositoryBase<User> _userRepository;
        public CustomerCreateModel()
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
                Status = 1,
            };

			_userRepository.Add(user);
            return RedirectToPage("/CustomerManagement");
        }
    }
}
