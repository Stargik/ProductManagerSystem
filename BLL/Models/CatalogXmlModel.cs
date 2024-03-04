using System;
using DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using System.Xml;

namespace BLL.Models
{

    [XmlRoot("catalog")]
    public class CatalogXmlModel
    {
        [XmlAttribute("date")]
        public string Date { get; set; }

        [XmlElement("shop")]
        public ShopXmlModel Shop { get; set; }
    }

    [XmlRoot("shop")]
    public class ShopXmlModel
    {
        [XmlArray("currencies")]
        [XmlArrayItem("currency")]
        public List<CurrencyTypeXmlModel> CurrencyTypes { get; set; }

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
        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlElement("manufacturer_code")]
        public string ManufacturerCode { get; set; }
        [XmlElement("stock_status")]
        public string StockStatus { get; set; }
        [XmlIgnore]
        public int StockQuantity { get; set; }
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
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlText]
        public string ValueNumber { get; set; }
        [XmlAttribute("unit")]
        public string? UnitType { get; set; }
    }

    [XmlRoot("category")]
    public class CategoryXmlModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlIgnore]
        public string? RozetkaId { get; set; }
        [XmlText]
        public string Title { get; set; }
    }

    [XmlRoot("currency")]
    public class CurrencyTypeXmlModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlText]
        public string Name { get; set; }
        [XmlAttribute("rate")]
        public decimal Rate { get; set; }
    }

}

