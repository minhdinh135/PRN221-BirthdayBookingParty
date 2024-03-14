using Microsoft.AspNetCore.Authorization;

namespace PRN221_BirthdayBookingParty.Authorization
{
    public class SessionRequirementHandler : AuthorizationHandler<SessionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SessionRequirement requirement)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var userRole = session.GetString("USER_ROLE");

            if (userRole == requirement.RequiredRole)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
