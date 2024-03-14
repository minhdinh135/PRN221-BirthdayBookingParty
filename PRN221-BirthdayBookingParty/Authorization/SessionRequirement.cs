using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Authorization
{
    public class SessionRequirement : IAuthorizationRequirement
    {
        public string RequiredRole { get; }

        public SessionRequirement(string requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}
