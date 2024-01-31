using System;
namespace DAL.Entities
{
	public class Characteristic : BaseEntity
	{
		public double ValueNumber { get; set; }
		public string UnitType { get; set; }
		public int ProductId { get; set; }

		public Product Product { get; set; }
	}
}

