using System;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
	public class ImageRepository : Repository<Image>, IImageRepository
    {
		public ImageRepository(ProductManagerDbContext context)
            : base(context)
        {
		}
	}
}

