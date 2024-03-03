using System;
using System.Xml.Linq;
using System.Xml.Serialization;
using BLL.Configuration;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.Extensions.Options;

namespace BLL.Services
{
	public class ProductExportService : IExportService<Product>
	{
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageService imageService;
        private readonly ShopSettings shopSettings;

        public ProductExportService(IUnitOfWork unitOfWork, IImageService imageService, IOptions<ShopSettings> shopSettings)
        {
            this.unitOfWork = unitOfWork;
            this.imageService = imageService;
            this.shopSettings = shopSettings.Value;
        }

        public async Task WriteToAsync(Stream stream, IEnumerable<Product> entities, PortType portType = PortType.Default)
        {
            if (portType == PortType.Default)
            {
                await WriteDefaultXmlAsync(stream, entities);
            }
            
        }

        private async Task WriteDefaultXmlAsync(Stream stream, IEnumerable<Product> entities)
        {
            var catalogXmlModel = new CatalogXmlModel {
                ShopUrl = shopSettings.BaseUrl,
                ShopName = shopSettings.Name,
                Products = new List<ProductXmlModel>(),
                Categories = new List<CategoryXmlModel>(),
                CurrencyTypes = new List<CurrencyTypeXmlModel>()
            };

            foreach (var item in entities)
            {
                catalogXmlModel.Products.Add(await ToProductXmlModelAsync(item));
                if (!catalogXmlModel.Categories.Exists(c => c.Id == item.CategoryId))
                {
                    var categoryXmlModel = new CategoryXmlModel { Id = item.Category.Id, Title = item.Category.Title };
                    catalogXmlModel.Categories.Add(categoryXmlModel);
                }

                if (!catalogXmlModel.CurrencyTypes.Exists(c => c.Id == item.CurrencyTypeId))
                {
                    var currencyTypeXmlModel = new CurrencyTypeXmlModel { Id = item.CurrencyType.Id, Name = item.CurrencyType.Name, Rate = item.CurrencyType.Rate };
                    catalogXmlModel.CurrencyTypes.Add(currencyTypeXmlModel);
                }
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CatalogXmlModel));
            xmlSerializer.Serialize(stream, catalogXmlModel);
        }

        public async Task<ProductXmlModel> ToProductXmlModelAsync(Product product)
        {
            var directoryPath = await imageService.GetStoragePath();
            string fullPath = shopSettings.BaseUrl + directoryPath + "/" + product.MainImage.Path;

            ProductXmlModel productXmlModel = new ProductXmlModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                ManufacturerCode = product.ManufacturerCode,
                StockStatus = product.StockStatus.Name,
                CurrencyType = product.CurrencyType.Name,
                Category = product.Category.Id,
                Manufacturer = product.Manufacturer.Name,
                MainImage = fullPath
            };

            if (product.Characteristics.Any())
            {
                productXmlModel.Characteristics = new List<CharacteristicXmlModel>();
                foreach (var characteristic in product.Characteristics)
                {
                    var characteristicXmlModel = new CharacteristicXmlModel
                    {
                        Name = characteristic.Name,
                        ValueNumber = characteristic.ValueNumber,
                        UnitType = characteristic.UnitType
                    };

                    productXmlModel.Characteristics.Add(characteristicXmlModel);
                }
            }

            return productXmlModel;
        }
    }
}

