using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
	public class Manufacturer : BaseEntity
	{
        [Display(Name = "Заголовок")]
        public string Name { get; set; }

        [Display(Name = "Товари")]
        public List<Product> Products { get; set; } = new List<Product>();
    }
}

