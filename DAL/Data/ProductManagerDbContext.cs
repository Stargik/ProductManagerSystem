using System;
using DAL.Configuration;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
	public class ProductManagerDbContext : DbContext
	{
		public ProductManagerDbContext(DbContextOptions<ProductManagerDbContext> options)
			: base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Characteristic> Characteristics { get; set; }
		public DbSet<CurrencyType> CurrencyTypes { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<Manufacturer> Manufacturers { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<StockStatus> StockStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CurrencyTypeConfiguration());
			modelBuilder.ApplyConfiguration(new StockStatusConfiguration());
        }
    }
}

