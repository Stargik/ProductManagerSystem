using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Areas.Identity.Data;

namespace MVCWebApp.Configuration
{
	public static class MigrationManager
	{
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ProductManagerDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            return webApp;
        }
        public static WebApplication MigrateIdentityDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ProductManagerIdentityDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            return webApp;
        }
    }
}

