using System;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class StockStatusRepository : IStockStatusRepository
	{
        private readonly ProductManagerDbContext context;

        public StockStatusRepository(ProductManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<StockStatus>> GetAllAsync()
        {
            return await context.StockStatuses.ToListAsync();
        }

        public async Task<StockStatus> GetByIdAsync(int id)
        {
            return await context.StockStatuses.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}

