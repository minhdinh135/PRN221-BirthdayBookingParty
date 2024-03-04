using Models;

namespace Services.Interfaces
{
    public interface IUserService
    {
        User GetAccount(string email, string password);
    }
}
