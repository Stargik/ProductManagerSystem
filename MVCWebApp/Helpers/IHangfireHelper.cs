using System;
namespace MVCWebApp.Helpers
{
	public interface IHangfireHelper
	{
        Task SendCatalogToAllSubscribersJob();
    }
}

