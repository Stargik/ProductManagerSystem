using System;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
	public class CurrencyTypeService : ICurrencyTypeService
	{
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrencyTypeRepository currencyTypeRepository;

        public CurrencyTypeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            currencyTypeRepository = unitOfWork.CurrencyTypeRepository;
        }

        public async Task AddAsync(CurrencyType entity)
        {
            await currencyTypeRepository.AddAsync(entity);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(CurrencyType entity)
        {
            await currencyTypeRepository.DeleteAsync(entity);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            await currencyTypeRepository.DeleteByIdAsync(id);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CurrencyType>> GetAllAsync()
        {
            var currencyTypes = await currencyTypeRepository.GetAllAsync();
            return currencyTypes;
        }

        public async Task<CurrencyType> GetByIdAsync(int id)
        {
            var manufacturer = await currencyTypeRepository.GetByIdAsync(id);
            return manufacturer;
        }

        public async Task UpdateAsync(CurrencyType entity)
        {
            await currencyTypeRepository.UpdateAsync(entity);
            await unitOfWork.SaveAsync();
        }
    }
}

