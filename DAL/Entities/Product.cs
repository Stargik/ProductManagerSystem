using System;
namespace DAL.Entities
{
	public class Product : BaseEntity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string ManufacturerCode { get; set; }
        public int StockStatusId { get; set; }
        public int СurrencyTypeId { get; set; }
        public int CategoryId { get; set; }
        public int MainImageId { get; set; }
        public int ManufacturerId { get; set; }

        public StockStatus StockStatus { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public Category Category { get; set; }
        public Image MainImage { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public List<Image> Images { get; set; }
        public List<Characteristic> Characteristics { get; set; }

    }
}

