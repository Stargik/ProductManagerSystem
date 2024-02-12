using System;
using DAL.Entities;

namespace MVCWebApp.Models
{
	public class ProductsViewModel
	{
		public IEnumerable<Product> Products { get; set; }
		public int? SearchCategoryId { get; set; }
		public int? SearchManufacturerId { get; set; }
        public string? SearchTitle { get; set; }
    }
}

