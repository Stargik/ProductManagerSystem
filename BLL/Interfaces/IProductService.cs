﻿using System;
using BLL.Models;
using DAL.Entities;

namespace BLL.Interfaces
{
	public interface IProductService
	{
        Task AddAsync(ProductDTO productDTO);
        Task UpdateAsync(ProductDTO productDTO);
        Task DeleteByIdAsync(int id);
        Task DeleteAsync(ProductDTO productDTO);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetByFilterAsync(FilterSearchModel filterSearch);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task RemoveCategoryByIdAsync(int categoryId);
        Task<IEnumerable<Characteristic>> GetAllCharacteristicByProductIdAsync(int productId);
        Task AddCharacteristicAsync(Characteristic characteristic);
        Task UpdateCharacteristicAsync(Characteristic characteristic);
        Task RemoveCharacteristicByIdAsync(int characteristicId);
    }
}
