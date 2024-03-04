using System;
using System.Text.RegularExpressions;
using System.Xml;
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
            if (portType == PortType.RozetkaXml)
            {
                await WriteRozetkaXmlAsync(stream, entities);
            }

        }

        private async Task WriteDefaultXmlAsync(Stream stream, IEnumerable<Product> entities)
        {
            var shopXmlModel = new ShopXmlModel
            {
                Products = new List<ProductXmlModel>(),
                Categories = new List<CategoryXmlModel>(),
                CurrencyTypes = new List<CurrencyTypeXmlModel>()
            };

            foreach (var item in entities)
            {
                shopXmlModel.Products.Add(await ToProductXmlModelAsync(item));
                if (!shopXmlModel.Categories.Exists(c => c.Id == item.CategoryId))
                {
                    var categoryXmlModel = new CategoryXmlModel { Id = item.Category.Id, Title = item.Category.Title };
                    shopXmlModel.Categories.Add(categoryXmlModel);
                }

                if (!shopXmlModel.CurrencyTypes.Exists(c => c.Id == item.CurrencyTypeId))
                {
                    var currencyTypeXmlModel = new CurrencyTypeXmlModel { Id = item.CurrencyType.Id, Name = item.CurrencyType.Name, Rate = item.CurrencyType.Rate };
                    shopXmlModel.CurrencyTypes.Add(currencyTypeXmlModel);
                }
            }

            var catalogXmlModel = new CatalogXmlModel
            {
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Shop = shopXmlModel
            };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CatalogXmlModel));
            xmlSerializer.Serialize(stream, catalogXmlModel, ns);
        }

        private async Task WriteRozetkaXmlAsync(Stream stream, IEnumerable<Product> entities)
        {
            var shopXmlModel = new ShopXmlModel
            {
                Products = new List<ProductXmlModel>(),
                Categories = new List<CategoryXmlModel>(),
                CurrencyTypes = new List<CurrencyTypeXmlModel>()
            };

            foreach (var item in entities)
            {
                if (item.StockStatus.StatusCode == 1 || item.StockStatus.StatusCode == 2)
                {
                    item.StockStatus.Name = "true";
                }
                else
                {
                    item.StockStatus.Name = "false";
                }
                item.Description = $"![CDATA[{item.Description}]]";

                foreach (var characteristic in item.Characteristics)
                {
                    characteristic.ValueNumber = $"{characteristic.ValueNumber} {characteristic.UnitType}";
                }

                shopXmlModel.Products.Add(await ToProductXmlModelAsync(item));

                if (!shopXmlModel.Categories.Exists(c => c.Id == item.CategoryId))
                {
                    var categoryXmlModel = new CategoryXmlModel { Id = item.Category.Id, Title = item.Category.Title, RozetkaId = item.Category.RozetkaId };
                    shopXmlModel.Categories.Add(categoryXmlModel);
                }

                if (!shopXmlModel.CurrencyTypes.Exists(c => c.Id == item.CurrencyTypeId))
                {
                    var currencyTypeXmlModel = new CurrencyTypeXmlModel { Id = item.CurrencyType.Id, Name = item.CurrencyType.Name, Rate = item.CurrencyType.Rate };
                    shopXmlModel.CurrencyTypes.Add(currencyTypeXmlModel);
                }
            }

            var catalogRozetkaXmlModel = new CatalogXmlModel
            {
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Shop = shopXmlModel
            };

            XmlAttributeOverrides xOver = new XmlAttributeOverrides();

            await RozetkaCatalogOverridingConfigure(xOver);

            await RozetkaShopOverridingConfigure(xOver);

            await RozetkaCategoryOverridingConfigure(xOver);

            await RozetkaCurrencyTypeOverridingConfigure(xOver);

            await RozetkaProductOverridingConfigure(xOver);

            await RozetkaCharacteristicOverridingConfigure(xOver);

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CatalogXmlModel), xOver);
            xmlSerializer.Serialize(stream, catalogRozetkaXmlModel, ns);
        }


        public async Task<ProductXmlModel> ToProductXmlModelAsync(Product product)
        {
            var directoryPath = await imageService.GetStoragePath();
            string fullPath = shopSettings.BaseUrl + directoryPath + "/" + product.MainImage.Path;
            int stockQuantity = (product.StockStatus.StatusCode == 1 || product.StockStatus.StatusCode == 2) ? 100 : 0;

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
                MainImage = fullPath,
                StockQuantity = stockQuantity
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

        private async Task RozetkaCatalogOverridingConfigure(XmlAttributeOverrides xOver)
        {
            XmlAttributes attrs = new XmlAttributes();
            XmlRootAttribute xRootAttribute = new XmlRootAttribute("yml_catalog");
            attrs.XmlRoot = xRootAttribute;
            xOver.Add(typeof(CatalogXmlModel), attrs);
        }

        private async Task RozetkaShopOverridingConfigure(XmlAttributeOverrides xOver)
        {
            XmlAttributes attrs = new XmlAttributes();
            XmlArrayAttribute xArray = new XmlArrayAttribute("offers");
            attrs.XmlArray = xArray;
            XmlArrayItemAttribute xArrItemElement = new XmlArrayItemAttribute("offer");
            attrs.XmlArrayItems.Add(xArrItemElement);
            xOver.Add(typeof(ShopXmlModel), "Products", attrs);
        }

        private async Task RozetkaCategoryOverridingConfigure(XmlAttributeOverrides xOver)
        {
            XmlAttributes attrs = new XmlAttributes();
            XmlAttributeAttribute xAttribute = new XmlAttributeAttribute("rz_id");
            attrs.XmlAttribute = xAttribute;
            xOver.Add(typeof(CategoryXmlModel), "RozetkaId", attrs);
        }

        private async Task RozetkaCurrencyTypeOverridingConfigure(XmlAttributeOverrides xOver)
        {
            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlIgnore = true;
            xOver.Add(typeof(CurrencyTypeXmlModel), "Id", attrs);

            attrs = new XmlAttributes();
            XmlAttributeAttribute xAttribute = new XmlAttributeAttribute("id");
            attrs.XmlAttribute = xAttribute;
            xOver.Add(typeof(CurrencyTypeXmlModel), "Name", attrs);
        }

        private async Task RozetkaCharacteristicOverridingConfigure(XmlAttributeOverrides xOver)
        {
            XmlAttributes attrs = new XmlAttributes();
            XmlAttributeAttribute xAttribute = new XmlAttributeAttribute("name");
            attrs.XmlAttribute = xAttribute;
            xOver.Add(typeof(CharacteristicXmlModel), "Name", attrs);

            attrs = new XmlAttributes();
            attrs.XmlIgnore = true;
            xOver.Add(typeof(CharacteristicXmlModel), "UnitType", attrs);
        }

        private async Task RozetkaProductOverridingConfigure(XmlAttributeOverrides xOver)
        {
            XmlAttributes attrs = new XmlAttributes();
            XmlElementAttribute xElement = new XmlElementAttribute("name");
            attrs.XmlElements.Add(xElement);
            xOver.Add(typeof(ProductXmlModel), "Title", attrs);

            attrs = new XmlAttributes();
            xElement = new XmlElementAttribute("categoryId");
            attrs.XmlElements.Add(xElement);
            xOver.Add(typeof(ProductXmlModel), "Category", attrs);

            attrs = new XmlAttributes();
            xElement = new XmlElementAttribute("vendor");
            attrs.XmlElements.Add(xElement);
            xOver.Add(typeof(ProductXmlModel), "Manufacturer", attrs);

            attrs = new XmlAttributes();
            xElement = new XmlElementAttribute("param");
            attrs.XmlElements.Add(xElement);
            xOver.Add(typeof(ProductXmlModel), "Characteristics", attrs);

            attrs = new XmlAttributes();
            xElement = new XmlElementAttribute("currencyId");
            attrs.XmlElements.Add(xElement);
            xOver.Add(typeof(ProductXmlModel), "CurrencyType", attrs);

            attrs = new XmlAttributes();
            xElement = new XmlElementAttribute("picture");
            attrs.XmlElements.Add(xElement);
            xOver.Add(typeof(ProductXmlModel), "MainImage", attrs);

            attrs = new XmlAttributes();
            xElement = new XmlElementAttribute("article");
            attrs.XmlElements.Add(xElement);
            xOver.Add(typeof(ProductXmlModel), "ManufacturerCode", attrs);

            attrs = new XmlAttributes();
            attrs.XmlIgnore = false;
            xElement = new XmlElementAttribute("stock_quantity");
            attrs.XmlElements.Add(xElement);
            xOver.Add(typeof(ProductXmlModel), "StockQuantity", attrs);

            attrs = new XmlAttributes();
            XmlAttributeAttribute xAttribute = new XmlAttributeAttribute("available");
            attrs.XmlAttribute = xAttribute;
            xOver.Add(typeof(ProductXmlModel), "StockStatus", attrs);
        }
    }
}

