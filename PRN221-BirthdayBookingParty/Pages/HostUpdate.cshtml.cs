using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Interfaces;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "AdminSessionPolicy")]

    public class HostUpdateModel : PageModel
    {
            private readonly IRepositoryBase<User> _userRepository;

            [BindProperty]
            public int UserId { get; set; }

            [BindProperty]
            public string FullName { get; set; }

            [BindProperty]
            public string Telephone { get; set; }

            [BindProperty]
            public string EmailAddress { get; set; }

            [BindProperty]
            public string Address { get; set; }

            [BindProperty]
            public DateTime Birthday { get; set; }

            public HostUpdateModel(IRepositoryBase<User> userRepository)
            {
                _userRepository = userRepository;
            }

            public IActionResult OnGet(int id)
            {
                User user = _userRepository.GetAll().FirstOrDefault(u => u.UserId == id);
                if (user != null)
                {
                    UserId = user.UserId;
                    FullName = user.FullName;
                    Telephone = user.Phone;
                    EmailAddress = user.Email;
                    Address = user.Address;
                    Birthday = user.Birthday;
                    return Page();
                }
                else
                {
                    return NotFound();
                }
            }

            public IActionResult OnPost()
            {
                User userToUpdate = _userRepository.GetAll().FirstOrDefault(u => u.UserId == UserId);
                if (userToUpdate == null)
                {
                    return NotFound();
                }

                userToUpdate.FullName = FullName;
                userToUpdate.Phone = Telephone;
                userToUpdate.Email = EmailAddress;
                userToUpdate.Address = Address;
                userToUpdate.Birthday = Birthday;

                _userRepository.Update(userToUpdate);

                return RedirectToPage("/HostManagement");
            }
        }
}
