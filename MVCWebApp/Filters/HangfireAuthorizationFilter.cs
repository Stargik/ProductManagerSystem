using System;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using MVCWebApp.Areas.Identity.Configuration;

namespace MVCWebApp.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
	{
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.IsInRole(RoleSettings.AdminRole);
        }
    }
}

