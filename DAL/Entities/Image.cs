using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
	public class Image : BaseEntity
	{
        [Display(Name = "Назва")]
        public string Path { get; set; }

        [Display(Name = "Товар")]
        public int? ProductId { get; set; }
        [Display(Name = "Товар")]
        public Product Product { get; set; }
    }
}

