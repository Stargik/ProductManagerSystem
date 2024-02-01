using System;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
	public class CharacteristicRepository : Repository<Characteristic>, ICharacteristicRepository
    {
		public CharacteristicRepository(ProductManagerDbContext context)
            : base(context)
        {
		}
	}
}

