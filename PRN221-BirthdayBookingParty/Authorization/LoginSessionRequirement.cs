using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Authorization
{
    public class LoginSessionRequirement : IAuthorizationRequirement
    {
        public bool IsLogin {  get; }

        public LoginSessionRequirement(bool isLogin)
        {
            IsLogin = isLogin;
        }
    }
}
