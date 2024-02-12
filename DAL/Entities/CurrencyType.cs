using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
	public class CurrencyType : BaseEntity
	{
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Товари")]
        public List<Product> Products { get; set; }
	}
}

