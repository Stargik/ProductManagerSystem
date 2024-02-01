using System;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class ProductRepository : Repository<Product>, IProductRepository
    {
		public ProductRepository(ProductManagerDbContext context)
            : base(context)
        {
		}

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await context.Products
                .Include(p => p.Images)
                .Include(p => p.Characteristics)
                .Include(p => p.MainImage)
                .Include(p => p.CurrencyType)
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.StockStatus)
                .ToListAsync();
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await context.Products
                .Include(p => p.Images)
                .Include(p => p.Characteristics)
                .Include(p => p.MainImage)
                .Include(p => p.CurrencyType)
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.StockStatus)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}

