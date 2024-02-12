using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
	public class StockStatus : BaseEntity
	{
        [Display(Name = "Заголовок")]
        public string Name { get; set; }
        [Display(Name = "Код")]
        public int StatusCode { get; set; }

        [Display(Name = "Товари")]
        public List<Product> Products { get; set; }
	}
}

