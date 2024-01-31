using System;
using DAL.Entities;

namespace DAL.Interfaces
{
	public interface IProductRepository : IRepository<Product>
	{
        Task<IEnumerable<Product>> GetAllWithDetailsAsync();
        Task<Product> GetByIdWithDetailsAsync(int id);
    }
}

