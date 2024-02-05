using System;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
	public class ManufacturerService : IManufacturerService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IManufacturerRepository manufacturerRepository;

		public ManufacturerService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
            manufacturerRepository = unitOfWork.ManufacturerRepository;
		}

        public async Task AddAsync(Manufacturer entity)
        {
            await manufacturerRepository.AddAsync(entity);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Manufacturer entity)
        {
            await manufacturerRepository.DeleteAsync(entity);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            await manufacturerRepository.DeleteByIdAsync(id);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Manufacturer>> GetAllAsync()
        {
            var manufacturers = await manufacturerRepository.GetAllAsync();
            return manufacturers;
        }

        public async Task<Manufacturer> GetByIdAsync(int id)
        {
            var manufacturer = await manufacturerRepository.GetByIdAsync(id);
            return manufacturer;
        }

        public async Task UpdateAsync(Manufacturer entity)
        {
            await manufacturerRepository.UpdateAsync(entity);
            await unitOfWork.SaveAsync();
        }
    }
}

