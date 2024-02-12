using System;
using DAL.Entities;

namespace DAL.Interfaces
{
	public interface IStockStatusRepository
	{
        Task<IEnumerable<StockStatus>> GetAllAsync();
        Task<StockStatus> GetByIdAsync(int id);
    }
}

