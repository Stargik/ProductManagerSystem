using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Areas.Identity.Data;
using DAL.Data;
using BLL.Configuration;
using MVCWebApp.Configuration;
using DAL.Interfaces;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.Options;
using MVCWebApp.Areas.Identity.Models;
using DAL.Entities;
using Hangfire;
using MVCWebApp.Helpers;
using MVCWebApp.Filters;

namespace MVCWebApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ProductManagerDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString(SettingStrings.ProductManagerDbConnection),
            providerOptions => providerOptions.EnableRetryOnFailure())
        );

        builder.Services.AddDbContext<ProductManagerIdentityDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString(SettingStrings.IdentityProductManagerDbConnection),
            providerOptions => providerOptions.EnableRetryOnFailure())
        );

        builder.Services.Configure<StaticFilesSettings>(builder.Configuration.GetSection(SettingStrings.StaticFilesSection));
        builder.Services.Configure<BlobStaticFilesSettings>(builder.Configuration.GetSection(SettingStrings.AzureBlobStorageSection));
        builder.Services.Configure<ShopSettings>(builder.Configuration.GetSection(SettingStrings.ShopSection));
        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(SettingStrings.MailSettings));
        builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection(SettingStrings.PaginationSettings));

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddTransient<IManufacturerService, ManufacturerService>();
        builder.Services.AddTransient<ICurrencyTypeService, CurrencyTypeService>();
        builder.Services.AddTransient<IProductService, ProductService>();

        if (builder.Configuration[SettingStrings.ImagesSetting] == "blob")
        {
            builder.Services.AddTransient<IImageService, BlobStorageImageService>();
        }
        else
        {
            builder.Services.AddTransient<IImageService, FilesystemImageService>(
                serviceProvider => new FilesystemImageService(
                    serviceProvider.GetRequiredService<IOptions<StaticFilesSettings>>(),
                    serviceProvider.GetService<IWebHostEnvironment>().WebRootPath
                )
            );
        }


        builder.Services.AddScoped<IDataPortServiceFactory<Product>, ProductDataPortServiceFactory>();
        builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();
        builder.Services.AddTransient<IHangfireHelper, HangfireHelper>();




        builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ProductManagerIdentityDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

        builder.Services.AddHangfire(x =>
        {
            x.UseSqlServerStorage(builder.Configuration.GetConnectionString(SettingStrings.HangfireProductManagerDbConnection));
        });

        builder.Services.AddHangfireServer();

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

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "Admin",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MigrateDatabase();
        app.MigrateIdentityDatabase();

        await app.InitializeRoles();

        app.UseHangfireDashboard("/Admin/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        });

        app.MapRazorPages();

        app.Run();
    }
}

