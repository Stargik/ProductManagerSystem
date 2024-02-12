﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(ProductManagerDbContext))]
    [Migration("20240209153307_InitialCreate3")]
    partial class InitialCreate3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DAL.Entities.Characteristic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("UnitType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ValueNumber")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Characteristics");
                });

            modelBuilder.Entity("DAL.Entities.CurrencyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CurrencyTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "EUR"
                        },
                        new
                        {
                            Id = 2,
                            Name = "USD"
                        },
                        new
                        {
                            Id = 3,
                            Name = "UAH"
                        });
                });

            modelBuilder.Entity("DAL.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("DAL.Entities.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("DAL.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("CurrencyTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MainImageId")
                        .HasColumnType("int");

                    b.Property<string>("ManufacturerCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ManufacturerId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StockStatusId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CurrencyTypeId");

                    b.HasIndex("MainImageId");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("StockStatusId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DAL.Entities.StockStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("StockStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Є в наявності",
                            StatusCode = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Закінчується",
                            StatusCode = 2
                        },
                        new
                        {
                            Id = 3,
                            Name = "Немає в наявності",
                            StatusCode = 3
                        },
                        new
                        {
                            Id = 4,
                            Name = "Очікується",
                            StatusCode = 4
                        },
                        new
                        {
                            Id = 5,
                            Name = "Під замовлення",
                            StatusCode = 5
                        });
                });

            modelBuilder.Entity("DAL.Entities.Characteristic", b =>
                {
                    b.HasOne("DAL.Entities.Product", "Product")
                        .WithMany("Characteristics")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DAL.Entities.Image", b =>
                {
                    b.HasOne("DAL.Entities.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DAL.Entities.Product", b =>
                {
                    b.HasOne("DAL.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.CurrencyType", "CurrencyType")
                        .WithMany("Products")
                        .HasForeignKey("CurrencyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Image", "MainImage")
                        .WithMany()
                        .HasForeignKey("MainImageId");

                    b.HasOne("DAL.Entities.Manufacturer", "Manufacturer")
                        .WithMany("Products")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.StockStatus", "StockStatus")
                        .WithMany("Products")
                        .HasForeignKey("StockStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("CurrencyType");

                    b.Navigation("MainImage");

                    b.Navigation("Manufacturer");

                    b.Navigation("StockStatus");
                });

            modelBuilder.Entity("DAL.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("DAL.Entities.CurrencyType", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("DAL.Entities.Manufacturer", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("DAL.Entities.Product", b =>
                {
                    b.Navigation("Characteristics");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("DAL.Entities.StockStatus", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
