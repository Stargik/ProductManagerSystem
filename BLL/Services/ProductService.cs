﻿using System;
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

            await productRepository.DeleteAsync(product);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            await productRepository.DeleteByIdAsync(id);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await productRepository.GetAllWithDetailsAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = await productRepository.GetAllWithDetailsAsync();
            products = products?.Where(p => p.CategoryId == filterSearch.CategoryId);
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
                await imageService.Upload(productDTO.MainImage);
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

        private async Task<Product> GetProductFromDTO(ProductDTO productDTO)
        {
            Image mainImage = new Image { Path = productDTO.MainImage.FileName };

            Product product = new Product
            {
                Title = productDTO.Title,
                Description = productDTO.Description,
                Price = productDTO.Price,
                ManufacturerCode = productDTO.ManufacturerCode,
                StockStatusId = productDTO.StockStatusId,
                СurrencyTypeId = productDTO.СurrencyTypeId,
                CategoryId = productDTO.CategoryId,
                ManufacturerId = productDTO.ManufacturerId,
                MainImage = mainImage
            };

            return product;   
        }
    }
}
