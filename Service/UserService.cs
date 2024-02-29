using Models;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        public UserService(IUserRepository userRepository) {
            this.userRepository = userRepository;
        }

        public User GetAccount(string email, string password)
        {
            userRepository = new UserRepository();
            var user = userRepository.GetAll().FirstOrDefault(u => email.Equals(u.Email) && password.Equals(u.Password));
            return user;
        }
    }
}
