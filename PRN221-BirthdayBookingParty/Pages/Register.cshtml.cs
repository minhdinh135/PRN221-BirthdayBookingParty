using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;

namespace PRN221_BirthdayBookingParty.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public DateTime Birthday { get; set; }
        [BindProperty]
        public byte? Status { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private readonly IRepositoryBase<User> userRepository;

        public RegisterModel(IRepositoryBase<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User user = new User
            {
                FullName = FullName,
                Email = Email,
                Phone = Phone,
                Address = Address,
                Birthday = DateTime.Parse(Birthday.ToString()),
                Status = 1,
                Password = Password,
                RoleId = 3
            };

            userRepository.Add(user);

            return RedirectToPage("/Login");
        }
    }
}