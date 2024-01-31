using System;
namespace DAL.Entities
{
	public class StockStatus : BaseEntity
	{
		public string Name { get; set; }

		public List<Product> Products { get; set; }
	}
}

