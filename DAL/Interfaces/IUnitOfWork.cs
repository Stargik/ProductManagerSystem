using System;
namespace DAL.Interfaces
{
	public interface IUnitOfWork
	{
		ICategoryRepository CategoryRepository { get; }
		ICharacteristicRepository CharacteristicRepository { get; }
		IImageRepository ImageRepository { get; }
		IManufacturerRepository ManufacturerRepository { get; }
		IProductRepository ProductRepository { get; }
        ICurrencyTypeRepository CurrencyTypeRepository { get; }
        IStockStatusRepository StockStatusRepository { get; }
        Task SaveAsync();
	}
}

