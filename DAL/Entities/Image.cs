using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
	public class Image : BaseEntity
	{
		public string Path { get; set; }

        [ForeignKey("MainProduct")]
        public int? MainProductId { get; set; }
        [ForeignKey("ProductId")]
        public int? ProductId { get; set; }

        public Product MainProduct { get; set; }
        public Product Product { get; set; }
    }
}

