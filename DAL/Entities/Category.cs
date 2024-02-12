using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
	public class Category : BaseEntity
	{
		[Display(Name = "Заголовок")]
		public string Title { get; set; }

        [Display(Name = "Товари")]
        public List<Product> Products { get; set; } = new List<Product>();
    }
}

