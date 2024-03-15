using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "AdminSessionPolicy")]

    public class HostUpdateModel : PageModel
    {
            private readonly IRepositoryBase<User> _userRepository;

            [BindProperty]
            public int UserId { get; set; }

            [BindProperty]
            [StringLength(40, MinimumLength = 2, ErrorMessage = "FullName must be between 2 and 40 characters")]
            [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "CustomerFullName can only contain alphabetic characters and spaces")]
            public string FullName { get; set; }

            [BindProperty]
            [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telephone must be a valid phone number")]
            public string Telephone { get; set; }

            [BindProperty]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email format")]
            public string EmailAddress { get; set; }

            [BindProperty]
            [StringLength(50, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 50 characters")]
            public string Address { get; set; }

            [BindProperty]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                DateTime birthday = new DateTime(Birthday.Year, Birthday.Month, Birthday.Day);
                int age = DateTime.Now.Year - Birthday.Year;
                if (age < 16)
                {
                    ModelState.AddModelError("Birthday", "You must be at least 16 years old.");
                    return Page();
                }
                if (age > 99)
                {
                    ModelState.AddModelError("Birthday", "You must not be over 100 years old.");
                    return Page();
                }
                if (IsDuplicate(Telephone, EmailAddress))
                {
                    ModelState.AddModelError(string.Empty, "Phone or Email already exists.");
                    return Page();
                }

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
            public bool IsDuplicate(string phone, string emailAddress)
            {
                var existingCustomer = _userRepository.GetAll().FirstOrDefault(c => c.Phone == phone || c.Email == emailAddress);
                return existingCustomer != null;
            }
    }
}
