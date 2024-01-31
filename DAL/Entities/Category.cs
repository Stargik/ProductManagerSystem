using System;
namespace DAL.Entities
{
	public class Category : BaseEntity
	{
		public string Title { get; set; }

		public List<Product> Products { get; set; }
	}
}

