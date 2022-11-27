using Hangfire.Dashboard;

namespace WebApi.Filters
{
    public class HandfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            // return httpContext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}
