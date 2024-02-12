
using System;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class CurrencyTypeRepository : ICurrencyTypeRepository
	{
        private readonly ProductManagerDbContext context;

        public CurrencyTypeRepository(ProductManagerDbContext context)
		{
			this.context = context;
		}

        public async Task<IEnumerable<CurrencyType>> GetAllAsync()
        {
            return await context.CurrencyTypes.ToListAsync();
        }

        public async Task<CurrencyType> GetByIdAsync(int id)
        {
            return await context.CurrencyTypes.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}

