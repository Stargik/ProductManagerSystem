using System;
using BLL.Configuration;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
    public class ProductDataPortServiceFactory : IDataPortServiceFactory<Product>
	{
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageService imageService;
        private readonly IOptions<ShopSettings> shopSettings;

        public ProductDataPortServiceFactory(IUnitOfWork unitOfWork, IImageService imageService, IOptions<ShopSettings> shopSettings)
        {
            this.unitOfWork = unitOfWork;
            this.imageService = imageService;
            this.shopSettings = shopSettings;
        }

        public IExportService<Product> GetExportService(string contentType)
        {
            if (contentType == "application/xml")
            {
                return new ProductExportService(unitOfWork, imageService, shopSettings);
            }

            throw new NotImplementedException();
        }
    }
}

