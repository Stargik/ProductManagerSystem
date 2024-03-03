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
                    Name = "EUR",
                    Rate = 40
                },
                new CurrencyType
                {
                    Id = 2,
                    Name = "USD",
                    Rate = 40
                },
                new CurrencyType
                {
                    Id = 3,
                    Name = "UAH",
                    Rate = 1
                }
            );
        }
    }
}

