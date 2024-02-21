using System.Data;
using System.Text;
using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using MVCWebApp.Areas.Identity.Configuration;
using MVCWebApp.Areas.Identity.Models;
using MVCWebApp.Helpers;

namespace MVCWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class SubscriptionController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IDataPortServiceFactory<Product> productDataServiceFactory;
        private readonly IProductService productService;
        private readonly ISubscriptionService subscriptionService;
        private readonly IRecurringJobManager recurringJobManager;
        private readonly IHangfireHelper hangfireHelper;

        public SubscriptionController(UserManager<User> userManager, IDataPortServiceFactory<Product> productDataServiceFactory, IProductService productService, ISubscriptionService subscriptionService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IHangfireHelper hangfireHelper)
        {
            this.userManager = userManager;
            this.productDataServiceFactory = productDataServiceFactory;
            this.productService = productService;
            this.subscriptionService = subscriptionService;
            this.recurringJobManager = recurringJobManager;
            this.hangfireHelper = hangfireHelper;
        }

        // GET: SubscriptionController
        public async Task<ActionResult> Index()
        {
            var subscribers = await userManager.GetUsersInRoleAsync(RoleSettings.SubscriberRole);
            return View(subscribers);
        }

        // GET: SubscriptionController/Edit
        public async Task<ActionResult> Edit()
        {
            return View();
        }

        // POST: SubscriptionController/Edit
        [HttpPost]
        public async Task<ActionResult> Edit(string CronString)
        {

            var subscribers = await userManager.GetUsersInRoleAsync(RoleSettings.SubscriberRole);
            try
            {
                recurringJobManager.AddOrUpdate("SendCatalogId", () => hangfireHelper.SendCatalogToAllSubscribersJob(), CronString);
                TempData["StatusMessage"] = "Автоматичне відправлення каталогу успішно налаштовано.";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Помилка. Неправильне значення CRON.";

            }

            return View();
        }


        // GET: SubscriptionController/Delete
        public async Task<ActionResult> Delete()
        {
            try
            {
                recurringJobManager.RemoveIfExists("SendCatalogId");
                TempData["StatusMessage"] = "Автоматичне відправлення каталогу вимкнено.";


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Помилка. Автоматичне відправлення каталогу не увімкнене.";

            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<ActionResult> Send(List<string> subscriberEmails)
        {
            if (subscriberEmails.Any())
            {
                string contentType = "application/xml";
                var exportService = productDataServiceFactory.GetExportService(contentType);
                byte[] file;

                using (var stream = new MemoryStream())
                {
                    var products = await productService.GetAllAsync();

                    await exportService.WriteToAsync(stream, products);

                    await stream.FlushAsync();

                    file = stream.ToArray();
                }

                var catalog = new EmailAttachmentModel
                {
                    File = file,
                    FileName = $"Catalog_{DateTime.UtcNow.ToString("MMddyyyy")}.xml",
                    FileType = "xml"
                };

                var message = "Надіслано на email: ";

                var sendedEmails = await subscriptionService.SendCatalogAsync(subscriberEmails, catalog);
                message += String.Join("; ", sendedEmails);

                TempData["StatusMessage"] = message;

            }
            return RedirectToAction(nameof(Index));
        }

    }
}
