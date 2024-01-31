using System;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
	public class StockStatusConfiguration : IEntityTypeConfiguration<StockStatus>
    {
        public void Configure(EntityTypeBuilder<StockStatus> builder)
        {
            builder.HasData(
                new StockStatus
                {
                    Id = 1,
                    StatusCode = 1,
                    Name = "Є в наявності"
                },
                new StockStatus
                {
                    Id = 2,
                    StatusCode = 2,
                    Name = "Закінчується"
                },
                new StockStatus
                {
                    Id = 3,
                    StatusCode = 3,
                    Name = "Немає в наявності"
                },
                new StockStatus
                {
                    Id = 4,
                    StatusCode = 4,
                    Name = "Очікується"
                },
                new StockStatus
                {
                    Id = 5,
                    StatusCode = 5,
                    Name = "Під замовлення"
                }
            );
        }
    }
}

