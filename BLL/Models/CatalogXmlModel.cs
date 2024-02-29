using System;
using DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace BLL.Models
{

    [XmlRoot("catalog")]
    public class CatalogXmlModel
    {

        [XmlElement("url")]
        public string ShopUrl { get; set; }

        [XmlElement("company")]
        public string ShopName { get; set; }

        [XmlArray("categories")]
        [XmlArrayItem("category")]
        public List<CategoryXmlModel> Categories { get; set; }

        [XmlArray("products")]
        [XmlArrayItem("product")]
        public List<ProductXmlModel> Products { get; set; }
    }

    [XmlRoot("product")]
    public class ProductXmlModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        public string Title { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlElement("manufacturer_code")]
        public string ManufacturerCode { get; set; }
        [XmlElement("stock_status")]
        public string StockStatus { get; set; }
        [XmlElement("category_id")]
        public int Category { get; set; }
        [XmlElement("brand")]
        public string Manufacturer { get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }
        [XmlElement("currency_type")]
        public string CurrencyType { get; set; }
        [XmlElement("main_image")]
        public string MainImage { get; set; }
        [XmlElement("images")]
        public List<string> Images { get; set; }
        [XmlArray("characteristics")]
        [XmlArrayItem("characteristic")]
        public List<CharacteristicXmlModel> Characteristics { get; set; }
    }

    [XmlRoot("characteristic")]
    public class CharacteristicXmlModel
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("value")]
        public string ValueNumber { get; set; }
        [XmlAttribute("unit")]
        public string? UnitType { get; set; }
    }

    [XmlRoot("category")]
    public class CategoryXmlModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlText]
        public string Title { get; set; }
    }


}

