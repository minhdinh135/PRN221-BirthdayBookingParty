using Models;
using Repositories;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : IUserService
    {
        UserRepository repository;
        public UserService() {
        }

        public User getAccount(string email, string password)
        {
            repository = new UserRepository();
            var user = repository.GetAll().FirstOrDefault(u => email.Equals(u.Email) && password.Equals(u.Password));
            return user;
        }
    }
}
