using System;
namespace DAL.Entities
{
	public class Manufacturer : BaseEntity
	{
		public string Name { get; set; }

		public List<Product> Products { get; set; }
	}
}

