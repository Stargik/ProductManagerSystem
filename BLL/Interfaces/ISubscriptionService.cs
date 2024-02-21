using System;
using BLL.Models;

namespace BLL.Interfaces
{
	public interface ISubscriptionService
	{
        Task<IEnumerable<string>> SendCatalogAsync(List<string> subscriberEmails, EmailAttachmentModel emailAttachmentModel);
    }
}

