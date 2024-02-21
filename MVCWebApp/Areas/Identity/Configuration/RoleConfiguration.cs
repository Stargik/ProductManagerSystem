using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVCWebApp.Areas.Identity.Models;

namespace MVCWebApp.Areas.Identity.Configuration
{
	public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = RoleSettings.UserRole,
                    NormalizedName = RoleSettings.NormalizedUserRole
                },
                new IdentityRole
                {
                    Name = RoleSettings.AdminRole,
                    NormalizedName = RoleSettings.NormalizedAdminRole
                },
                new IdentityRole
                {
                    Name = RoleSettings.SubscriberRole,
                    NormalizedName = RoleSettings.NormalizedSubscriberRole
                }
            );
        }

        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            string adminEmail = configuration.GetSection(SettingStrings.AdminSettings).GetSection(RoleSettings.Email).Value;
            string password = configuration.GetSection(SettingStrings.AdminSettings).GetSection(RoleSettings.Password).Value;

            if (await roleManager.FindByNameAsync(RoleSettings.AdminRole) is null)
            {
                await roleManager.CreateAsync(new IdentityRole(RoleSettings.AdminRole));
            }
            if (await roleManager.FindByNameAsync(RoleSettings.UserRole) is null)
            {
                await roleManager.CreateAsync(new IdentityRole(RoleSettings.UserRole));
            }
            if (await roleManager.FindByNameAsync(RoleSettings.SubscriberRole) is null)
            {
                await roleManager.CreateAsync(new IdentityRole(RoleSettings.SubscriberRole));
            }

            if (await userManager.FindByNameAsync(adminEmail) is null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(admin);
                    await userManager.ConfirmEmailAsync(admin, code);
                    await userManager.AddToRoleAsync(admin, RoleSettings.AdminRole);
                }
            }
        }

    }
}

