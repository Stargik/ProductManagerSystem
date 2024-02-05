using System;
using Microsoft.AspNetCore.Http;

namespace BLL.Models
{
	public class ProductDTO
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ManufacturerCode { get; set; }
        public int StockStatusId { get; set; }
        public int СurrencyTypeId { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public IFormFile? MainImage { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}

