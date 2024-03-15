using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Authorization
{
    public class LoginSessionRequirementHandler : AuthorizationHandler<LoginSessionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginSessionRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LoginSessionRequirement requirement)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var userRole = session.GetString("USER_ROLE");

            if(userRole != null)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
