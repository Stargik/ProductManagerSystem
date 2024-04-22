using System.Data;
using System.Text;
using System.Threading;
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
using MVCWebApp.Areas.Admin.Models;
using MVCWebApp.Areas.Identity.Configuration;
using MVCWebApp.Areas.Identity.Models;
using MVCWebApp.Helpers;
using NuGet.Configuration;

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
        public async Task<ActionResult> Edit(SubscriptionSettingsModel subscriptionSettingsModel)
        {


            string cronString = subscriptionSettingsToCron(subscriptionSettingsModel);

            var setterDateTime = subscriptionSettingsModel.StartDate;

            setterDateTime = setterDateTime.AddMinutes(-1);

            var subscribers = await userManager.GetUsersInRoleAsync(RoleSettings.SubscriberRole);
            try
            {
                var monitor = JobStorage.Current.GetMonitoringApi();

                var jobIdsToDelete = monitor.ScheduledJobs(0, int.MaxValue).Where(x => x.Value.Job.Method.Name == nameof(AddSendCatalogAction));
                foreach (var id in jobIdsToDelete)
                {
                    BackgroundJob.Delete(id.Key);
                }
                recurringJobManager.RemoveIfExists("SendCatalogId");

                if (setterDateTime < DateTime.Now.AddMinutes(1))
                {
                    AddSendCatalogAction(cronString);
                }
                else
                {
                    var dateTimeOffset = new DateTimeOffset(setterDateTime);
                    var jobId = BackgroundJob.Schedule(() => AddSendCatalogAction(cronString), dateTimeOffset);
                }

                TempData["StatusMessage"] = "Автоматичне відправлення каталогу успішно налаштовано.";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Помилка. Неправильне значення.";

            }

            return View();
        }


        // GET: SubscriptionController/Delete
        public async Task<ActionResult> Delete()
        {
            try
            {
                var monitor = JobStorage.Current.GetMonitoringApi();

                var jobIdsToDelete = monitor.ScheduledJobs(0, int.MaxValue).Where(x => x.Value.Job.Method.Name == nameof(AddSendCatalogAction));
                foreach (var id in jobIdsToDelete)
                {
                    BackgroundJob.Delete(id.Key);
                }

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

        private string subscriptionSettingsToCron(SubscriptionSettingsModel settings)
        {
            string cronString;
            switch (settings.PeriodType)
            {
                case PeriodType.Daily:
                    cronString = Cron.Daily(settings.StartDate.Hour, settings.StartDate.Minute);
                    break;
                case PeriodType.Weekly:
                    cronString = Cron.Weekly(settings.StartDate.DayOfWeek, settings.StartDate.Hour, settings.StartDate.Minute);
                    break;
                case PeriodType.Monthly:
                    cronString = Cron.Monthly(settings.StartDate.Day, settings.StartDate.Hour, settings.StartDate.Minute);
                    break;
                case PeriodType.Yearly:
                    cronString = Cron.Yearly(settings.StartDate.Month, settings.StartDate.Day, settings.StartDate.Hour, settings.StartDate.Minute);
                    break;
                default:
                    cronString = Cron.Never();
                    break;
            }
            return cronString;
        }

        public void AddSendCatalogAction(string cronString)
        {
            RecurringJobOptions options = new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            };

            recurringJobManager.AddOrUpdate("SendCatalogId", () => hangfireHelper.SendCatalogToAllSubscribersJob(), cronString, options);
        }

    }
}
