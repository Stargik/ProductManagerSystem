using System;
namespace BLL.Models
{
	public class FilterSearchModel
	{
		public int? CategoryId { get; set; }
        public int? ManufacturerId { get; set; }
        public string? SearchTitle { get; set; }
    }
}

