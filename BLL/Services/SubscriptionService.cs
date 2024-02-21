using System;
using System.Text;
using BLL.Interfaces;
using BLL.Models;

namespace BLL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IEmailService emailService;

        public SubscriptionService(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task<IEnumerable<string>> SendCatalogAsync(List<string> subscriberEmails, EmailAttachmentModel emailAttachmentModel)
        {
            var sendedEmails = new List<string>();

            foreach (var email in subscriberEmails)
            {
                await SendCatalogByEmailAsync(email, emailAttachmentModel);
                sendedEmails.Add(email);
            }

            return sendedEmails;
        }

        private async Task SendCatalogByEmailAsync(string subscriberEmail, EmailAttachmentModel catalog)
        {

            var attachments = new List<EmailAttachmentModel>() { catalog };

            string subject = "Актуальний каталог товарів";
            string message = "Дякуємо, що ви з нами. Надсилаємо актуальний каталог нашої продукції.";
            await emailService.SendEmailAsync(subscriberEmail, subject, message, attachments);
        }
    }
}

