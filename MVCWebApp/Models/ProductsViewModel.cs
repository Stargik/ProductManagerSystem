using System;
using BLL.Models;
using DAL.Entities;
using X.PagedList;

namespace MVCWebApp.Models
{
	public class ProductsViewModel
	{
		public IPagedList<Product> Products { get; set; }
		public int? SearchCategoryId { get; set; }
		public int? SearchManufacturerId { get; set; }
        public string? SearchTitle { get; set; }
		public SortProductsViewModel SortProductsViewModel { get; set; } = new SortProductsViewModel(ProductSortState.Default);
    }
}

