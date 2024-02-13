using System;
using System.Reflection.PortableExecutable;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
	public class ProductService : IProductService
	{
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly IImageService imageService;

        public ProductService(IUnitOfWork unitOfWork, IImageService imageService)
		{
            this.unitOfWork = unitOfWork;
            productRepository = unitOfWork.ProductRepository;
            this.imageService = imageService;
		}

        public async Task AddAsync(ProductDTO productDTO)
        {
            await imageService.Upload(productDTO.MainImage);

            var product = await GetProductFromDTO(productDTO);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(ProductDTO productDTO)
        {
            var product = await GetProductFromDTO(productDTO);
            await imageService.Remove(product.MainImage.Path);
            await unitOfWork.ImageRepository.DeleteByIdAsync((int)product.MainImageId);

            await productRepository.DeleteAsync(product);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var product = await productRepository.GetByIdWithDetailsAsync(id);
            await imageService.Remove(product.MainImage.Path);
            await unitOfWork.ImageRepository.DeleteByIdAsync((int)product.MainImageId);

            await productRepository.DeleteByIdAsync(id);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(ProductSortState productSortState = ProductSortState.Default)
        {
            var products = await productRepository.GetAllWithDetailsAsync();

            if (productSortState != ProductSortState.Default)
            {
                products = await GetSortedProducts(products, productSortState);
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetByFilterAsync(FilterSearchModel filterSearch, ProductSortState productSortState = ProductSortState.Default)
        {
            var products = await productRepository.GetAllWithDetailsAsync();
            products = products?.Where(p => (p.CategoryId == filterSearch.CategoryId || filterSearch.CategoryId is null) &&
                                       (p.ManufacturerId == filterSearch.ManufacturerId || filterSearch.ManufacturerId is null) &&
                                       (String.IsNullOrEmpty(filterSearch.SearchTitle) || p.Title.ToUpper().Contains(filterSearch.SearchTitle.ToUpper())));

            if (productSortState != ProductSortState.Default && products is not null)
            {
                products = await GetSortedProducts(products, productSortState);
            }

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdWithDetailsAsync(id);
            return product;
        }

        public async Task UpdateAsync(ProductDTO productDTO)
        {
            var product = await GetProductFromDTO(productDTO);
            if (productDTO.MainImage is null)
            {
                product.MainImage = (await productRepository.GetByIdWithDetailsAsync(productDTO.Id)).MainImage;
            }
            if (productDTO.MainImage is not null)
            {
                var oldImg = (await productRepository.GetByIdWithDetailsAsync(productDTO.Id)).MainImage;
                await imageService.Remove(oldImg.Path);

                await imageService.Upload(productDTO.MainImage);
                oldImg.Path = product.MainImage.Path;
                await unitOfWork.ImageRepository.UpdateAsync(oldImg);
            }

            await productRepository.UpdateAsync(product);
            await unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            await unitOfWork.CategoryRepository.AddAsync(category);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await unitOfWork.CategoryRepository.GetAllAsync();
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);
            return category;
        }

        public async Task RemoveCategoryByIdAsync(int categoryId)
        {
            await unitOfWork.CategoryRepository.DeleteByIdAsync(categoryId);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await unitOfWork.CategoryRepository.UpdateAsync(category);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Characteristic>> GetAllCharacteristicByProductIdAsync(int productId)
        {
            var characteristics = (await unitOfWork.CharacteristicRepository.GetAllAsync()).Where(c => c.ProductId == productId);
            return characteristics;
        }

        public async Task<Characteristic> GetCharacteristicByIdAsync(int id)
        {
            var characteristic = await unitOfWork.CharacteristicRepository.GetByIdAsync(id);
            return characteristic;
        }

        public async Task AddCharacteristicAsync(Characteristic characteristic)
        {
            await unitOfWork.CharacteristicRepository.AddAsync(characteristic);
            await unitOfWork.SaveAsync();
        }

        public async Task RemoveCharacteristicByIdAsync(int characteristicId)
        {
            await unitOfWork.CharacteristicRepository.DeleteByIdAsync(characteristicId);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateCharacteristicAsync(Characteristic characteristic)
        {
            await unitOfWork.CharacteristicRepository.UpdateAsync(characteristic);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<StockStatus>> GetAllStockStatusesAsync()
        {
            var stockStatuses = await unitOfWork.StockStatusRepository.GetAllAsync();
            return stockStatuses;
        }

        public async Task<ProductDTO> GetProductDTOByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            ProductDTO productDTO = new ProductDTO
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                ManufacturerCode = product.ManufacturerCode,
                StockStatusId = product.StockStatusId,
                CurrencyTypeId = product.CurrencyTypeId,
                CategoryId = product.CategoryId,
                ManufacturerId = product.ManufacturerId
            };

            return productDTO;
        }

        private async Task<Product> GetProductFromDTO(ProductDTO productDTO)
        {
            Image mainImage = new Image { Path = productDTO.MainImage?.FileName };

            Product product = new Product
            {
                Id = productDTO.Id,
                Title = productDTO.Title,
                Description = productDTO.Description,
                Price = productDTO.Price,
                ManufacturerCode = productDTO.ManufacturerCode,
                StockStatusId = productDTO.StockStatusId,
                CurrencyTypeId = productDTO.CurrencyTypeId,
                CategoryId = productDTO.CategoryId,
                ManufacturerId = productDTO.ManufacturerId,
                MainImage = mainImage
            };

            return product;   
        }

        private async Task<IEnumerable<Product>> GetSortedProducts(IEnumerable<Product> products, ProductSortState productSortState = ProductSortState.Default)
        {
            products = productSortState switch
            {
                ProductSortState.TitleAsc => products.OrderBy(p => p.Title),
                ProductSortState.TitleDesc => products.OrderByDescending(p => p.Title),
                ProductSortState.ManufacturerCodeAsc => products.OrderBy(p => p.ManufacturerCode),
                ProductSortState.ManufacturerCodeDesc => products.OrderByDescending(p => p.ManufacturerCode),
                ProductSortState.PriceAsc => products.OrderBy(p => p.Price),
                ProductSortState.PriceDesc => products.OrderByDescending(p => p.Price),
                _ => products.OrderBy(p => p.Id)
            };
            return products;
        }

        public async Task<IEnumerable<CurrencyType>> GetAllCurrencyTypesAsync()
        {
            var currencyTypes = await unitOfWork.CurrencyTypeRepository.GetAllAsync();
            return currencyTypes;
        }
    }
}

