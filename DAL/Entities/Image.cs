using System;
namespace DAL.Entities
{
	public class Image : BaseEntity
	{
		public string Path { get; set; }
		public int ProductId { get; set; }

		public Product Product { get; set; }
	}
}

