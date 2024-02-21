using BLL.Configuration;
using BLL.Interfaces;
using Microsoft.Extensions.Options;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using BLL.Models;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings settings;

        public EmailService(IOptions<MailSettings> settings)
        {
            this.settings = settings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message, IEnumerable<EmailAttachmentModel> attachments = null)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(settings.DisplayName, settings.From));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var builder = new BodyBuilder { TextBody = message };

            if (attachments is not null && attachments.Any())
            {
                foreach (var item in attachments)
                {
                    if (item is not null)
                    {
                        builder.Attachments.Add(item.FileName, item.File);
                    }
                }

            }

            emailMessage.Body = builder.ToMessageBody();


            using (var client = new SmtpClient())
            {
                if (settings.UseSSL)
                {
                    await client.ConnectAsync(settings.Host, settings.Port, SecureSocketOptions.SslOnConnect);
                }
                else if (settings.UseStartTls)
                {
                    await client.ConnectAsync(settings.Host, settings.Port, SecureSocketOptions.StartTls);
                }
                await client.AuthenticateAsync(settings.UserName, settings.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}

