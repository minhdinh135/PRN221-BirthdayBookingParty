using Models;
using Repositories.Impl;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
    }
}
