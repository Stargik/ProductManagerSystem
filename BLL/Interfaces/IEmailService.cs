using System;
using BLL.Models;

namespace BLL.Interfaces
{
	public interface IEmailService
	{
        public Task SendEmailAsync(string email, string subject, string message, IEnumerable<EmailAttachmentModel> attachments = null);
    }
}

