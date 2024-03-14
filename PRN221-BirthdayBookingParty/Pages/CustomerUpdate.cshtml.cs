using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NguyenHoangLamRazorPages.Pages
{
    [Authorize(Policy = "AdminSessionPolicy")]

    public class CustomerUpdateModel : PageModel
    {
        [BindProperty]
        public int UserId { get; set; }
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        [Phone]
        public string Telephone { get; set; }
        [BindProperty]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [BindProperty]
        public int RoleId { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
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
                RoleId = user.RoleId;
            }
        }

        public IActionResult OnPost()
        {
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
            userToUpdate.RoleId = 3;

            _userRepository.Update(userToUpdate);

            return RedirectToPage("/CustomerManagement");
        }
    }
}
