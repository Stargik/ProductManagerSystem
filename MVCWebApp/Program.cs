using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Areas.Identity.Data;
using DAL.Data;
using BLL.Configuration;
using MVCWebApp.Configuration;

namespace MVCWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ProductManagerDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString(SettingStrings.ProductManagerDbConnection))
        );

        builder.Services.AddDbContext<ProductManagerIdentityDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString(SettingStrings.IdentityProductManagerDbConnection))
        );

        builder.Services.Configure<StaticFilesSettings>(builder.Configuration.GetSection(SettingStrings.StaticFilesSection));


        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ProductManagerDbContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MigrateDatabase();
        app.MigrateIdentityDatabase();

        app.MapRazorPages();

        app.Run();
    }
}
