using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BLL.Models
{
	public class ProductDTO
	{
        public int Id { get; set; }
        [Display(Name = "Заголовок")]
        public string Title { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }
        [Display(Name = "Артикул")]
        public string ManufacturerCode { get; set; }
        [Display(Name = "Наявність товару")]
        public int StockStatusId { get; set; }
        [Display(Name = "Валюта")]
        public int CurrencyTypeId { get; set; }
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }
        [Display(Name = "Бренд")]
        public int ManufacturerId { get; set; }
        [Display(Name = "Зображення")]
        public IFormFile? MainImage { get; set; }
        [Display(Name = "Додаткові зображення")]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}

