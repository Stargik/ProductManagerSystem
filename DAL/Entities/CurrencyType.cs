using System;
namespace DAL.Entities
{
	public class CurrencyType : BaseEntity
	{
		public string Name { get; set; }

		public List<Product> Products { get; set; }
	}
}

