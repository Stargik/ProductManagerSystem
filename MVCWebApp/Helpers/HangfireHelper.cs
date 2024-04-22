using System;
using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using MVCWebApp.Areas.Identity.Configuration;
using MVCWebApp.Areas.Identity.Models;

namespace MVCWebApp.Helpers
{
    public class HangfireHelper : IHangfireHelper
	{
        private readonly UserManager<User> userManager;
        private readonly IDataPortServiceFactory<Product> productDataServiceFactory;
        private readonly IProductService productService;
        private readonly ISubscriptionService subscriptionService;

        public HangfireHelper(UserManager<User> userManager, IDataPortServiceFactory<Product> productDataServiceFactory, IProductService productService, ISubscriptionService subscriptionService)
        {
            this.userManager = userManager;
            this.productDataServiceFactory = productDataServiceFactory;
            this.productService = productService;
            this.subscriptionService = subscriptionService;
        }

        public async Task SendCatalogToAllSubscribersJob()
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

            var subscribers = new List<string>((await userManager.GetUsersInRoleAsync(RoleSettings.SubscriberRole)).Select(user => user.Email));

            var sendedEmails = await subscriptionService.SendCatalogAsync(subscribers, catalog);
        }

    }
}

