using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
	public class Product : BaseEntity
	{
        [Display(Name = "Заголовок")]
        public string Title { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }
        [Display(Name = "Артикул")]
        public string ManufacturerCode { get; set; }
        [Display(Name = "Наявність")]
        public int StockStatusId { get; set; }
        public int CurrencyTypeId { get; set; }
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }
        [Display(Name = "Зображення")]
        [ForeignKey("MainImage")]
        public int? MainImageId { get; set; }
        [Display(Name = "Бренд")]
        public int ManufacturerId { get; set; }

        [Display(Name = "Наявність")]
        public StockStatus StockStatus { get; set; }
        public CurrencyType CurrencyType { get; set; }
        [Display(Name = "Категорія")]
        public Category Category { get; set; }
        [Display(Name = "Зображення")]
        public Image MainImage { get; set; }
        [Display(Name = "Бренд")]
        public Manufacturer Manufacturer { get; set; }
        [Display(Name = "Додаткові зображення")]
        [InverseProperty("Product")]
        public List<Image> Images { get; set; }
        [Display(Name = "Характеристики")]
        public List<Characteristic> Characteristics { get; set; } = new List<Characteristic>();

    }
}

