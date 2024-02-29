using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;

namespace NguyenHoangLamRazorPages.Pages
{
    public class CustomerUpdateModel : PageModel
    {
        [BindProperty]
        public int UserId { get; set; }
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string Telephone { get; set; }
        [BindProperty]
        public string EmailAddress { get; set; }
        [BindProperty]
        public DateTime Birthday { get; set; }

        private IRepositoryBase<User> _userRepository;
        public CustomerUpdateModel()
        {
			_userRepository = new UserRepository();
        }

        public void OnGet(int id)
        {
            User user = _userRepository.GetAll().FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                UserId = user.UserId;
                FullName = user.FullName;
                Telephone = user.Phone;
                EmailAddress = user.Email;
                Birthday = user.Birthday;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User userToUpdate = _userRepository.GetAll().FirstOrDefault(u => u.UserId == UserId);
            if (userToUpdate == null)
            {
                return NotFound();
            }

			userToUpdate.UserId = UserId;
			userToUpdate.FullName = FullName;
			userToUpdate.Phone = Telephone;
			userToUpdate.Email = EmailAddress;
			userToUpdate.Birthday = Birthday;
			userToUpdate.Status = 1;

            _userRepository.Update(userToUpdate);

            return RedirectToPage("/CustomerManagement");
        }
    }
}
