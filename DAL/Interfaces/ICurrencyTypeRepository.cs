using System;
using DAL.Entities;

namespace DAL.Interfaces
{
	public interface ICurrencyTypeRepository
	{
        Task<IEnumerable<CurrencyType>> GetAllAsync();
        Task<CurrencyType> GetByIdAsync(int id);
    }
}

