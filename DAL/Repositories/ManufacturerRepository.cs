using System;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
	public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
    {
		public ManufacturerRepository(ProductManagerDbContext context)
            : base(context)
        {
		}
	}
}

