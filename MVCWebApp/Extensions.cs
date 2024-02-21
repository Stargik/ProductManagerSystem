using System;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using MVCWebApp.Areas.Identity.Configuration;
using MVCWebApp.Areas.Identity.Models;
using MVCWebApp.Helpers;

namespace MVCWebApp
{
    public static class Extensions
    {
        public static async Task InitializeRoles(this WebApplication webApp)
        {

            using (var scope = webApp.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await RoleConfiguration.InitializeAsync(userManager, rolesManager, webApp.Configuration);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database." + DateTime.Now.ToString());
                }
            }
        }

        public static async Task InitializeSendingCatalogPeriodicallyJob(this WebApplication webApp)
        {

            using (var scope = webApp.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();
                var recurringJobManager = services.GetRequiredService<IRecurringJobManager>();
                var hangfireHelper = services.GetRequiredService<IHangfireHelper>();


                var subscribers = await userManager.GetUsersInRoleAsync(RoleSettings.SubscriberRole);

                recurringJobManager.AddOrUpdate("SendCatalogId", () => hangfireHelper.SendCatalogToAllSubscribersJob(), Cron.Minutely);

            }
        }
    }
}

