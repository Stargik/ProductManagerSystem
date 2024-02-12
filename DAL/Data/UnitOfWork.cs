using System;
using DAL.Interfaces;
using DAL.Repositories;

namespace DAL.Data
{
	public class UnitOfWork : IUnitOfWork
	{
        private readonly ProductManagerDbContext context;

		public UnitOfWork(ProductManagerDbContext context)
		{
            this.context = context;
		}

        private ICategoryRepository categoryRepository;
        private ICharacteristicRepository characteristicRepository;
        private IImageRepository imageRepository;
        private IManufacturerRepository manufacturerRepository;
        private IProductRepository productRepository;
        private ICurrencyTypeRepository currencyTypeRepository;
        private IStockStatusRepository stockStatusRepository;

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (categoryRepository is null)
                {
                    categoryRepository = new CategoryRepository(context);
                }
                return categoryRepository;
            }
        }

        public ICharacteristicRepository CharacteristicRepository
        {
            get
            {
                if (characteristicRepository is null)
                {
                    characteristicRepository = new CharacteristicRepository(context);
                }
                return characteristicRepository;
            }
        }

        public IImageRepository ImageRepository
        {
            get
            {
                if (imageRepository is null)
                {
                    imageRepository = new ImageRepository(context);
                }
                return imageRepository;
            }
        }

        public IManufacturerRepository ManufacturerRepository
        {
            get
            {
                if (manufacturerRepository is null)
                {
                    manufacturerRepository = new ManufacturerRepository(context);
                }
                return manufacturerRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository is null)
                {
                    productRepository = new ProductRepository(context);
                }
                return productRepository;
            }
        }

        public ICurrencyTypeRepository CurrencyTypeRepository
        {
            get
            {
                if (currencyTypeRepository is null)
                {
                    currencyTypeRepository = new CurrencyTypeRepository(context);
                }
                return currencyTypeRepository;
            }
        }

        public IStockStatusRepository StockStatusRepository
        {
            get
            {
                if (stockStatusRepository is null)
                {
                    stockStatusRepository = new StockStatusRepository(context);
                }
                return stockStatusRepository;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}

