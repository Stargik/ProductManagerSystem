
using System;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class CurrencyTypeRepository : Repository<CurrencyType>, ICurrencyTypeRepository
    {
        public CurrencyTypeRepository(ProductManagerDbContext context)
            : base(context)
        {
        }
    }
}

