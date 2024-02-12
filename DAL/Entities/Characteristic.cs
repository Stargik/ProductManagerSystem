using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
	public class Characteristic : BaseEntity
	{
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Значення")]
        public string ValueNumber { get; set; }
        [Display(Name = "Одиниця виміру")]
        public string? UnitType { get; set; }
        [Display(Name = "Товар")]
        public int ProductId { get; set; }

        [Display(Name = "Товар")]
        public Product? Product { get; set; } 
	}
}

