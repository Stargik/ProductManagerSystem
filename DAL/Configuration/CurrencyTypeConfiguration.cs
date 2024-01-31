using System;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class CurrencyTypeConfiguration : IEntityTypeConfiguration<CurrencyType>
    {
        public void Configure(EntityTypeBuilder<CurrencyType> builder)
        {
            builder.HasData(
                new CurrencyType
                {
                    Id = 1,
                    Name = "EUR"
                },
                new CurrencyType
                {
                    Id = 2,
                    Name = "USD"
                },
                new CurrencyType
                {
                    Id = 3,
                    Name = "UAH"
                }
            );
        }
    }
}

