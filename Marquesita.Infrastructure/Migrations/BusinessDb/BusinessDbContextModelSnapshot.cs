﻿// <auto-generated />
using System;
using Marquesita.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Marquesita.Infrastructure.Migrations.BusinessDb
{
    [DbContext(typeof(BusinessDbContext))]
    partial class BusinessDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Marquesita.Models.Business.Address", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varbinary(16)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("FullNames")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Marquesita.Models.Business.Categories", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varbinary(16)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Marquesita.Models.Business.Comments", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varbinary(16)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<byte[]>("ProductId")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Marquesita.Models.Business.Product", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varbinary(16)");

                    b.Property<byte[]>("CategoryId")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("ImageRoute")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Marquesita.Models.Business.Sale", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varbinary(16)");

                    b.Property<byte[]>("AddressId")
                        .HasColumnType("varbinary(16)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("SaleStatus")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("TypeOfSale")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Marquesita.Models.Business.SaleDetail", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varbinary(16)");

                    b.Property<byte[]>("ProductId")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<byte[]>("SaleId")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.Property<decimal>("Subtotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleDetails");
                });

            modelBuilder.Entity("Marquesita.Models.Business.SaleDetailTemp", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varbinary(16)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<byte[]>("ProductId")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("SaleDetailsTemp");
                });

            modelBuilder.Entity("Marquesita.Models.Business.ShoppingCart", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varbinary(16)");

                    b.Property<byte[]>("ProductId")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("Marquesita.Models.Business.WishList", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varbinary(16)");

                    b.Property<byte[]>("ProductId")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("WishLists");
                });

            modelBuilder.Entity("Marquesita.Models.Business.Comments", b =>
                {
                    b.HasOne("Marquesita.Models.Business.Product", "Product")
                        .WithMany("Comments")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_Comment_ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marquesita.Models.Business.Product", b =>
                {
                    b.HasOne("Marquesita.Models.Business.Categories", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_Product_CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marquesita.Models.Business.Sale", b =>
                {
                    b.HasOne("Marquesita.Models.Business.Address", "Address")
                        .WithMany("Sales")
                        .HasForeignKey("AddressId")
                        .HasConstraintName("FK_Sale_AddresId");
                });

            modelBuilder.Entity("Marquesita.Models.Business.SaleDetail", b =>
                {
                    b.HasOne("Marquesita.Models.Business.Product", "Product")
                        .WithMany("SaleDetails")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_SaleDetail_ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Marquesita.Models.Business.Sale", "Sale")
                        .WithMany("SaleDetails")
                        .HasForeignKey("SaleId")
                        .HasConstraintName("FK_SaleDetail_SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marquesita.Models.Business.SaleDetailTemp", b =>
                {
                    b.HasOne("Marquesita.Models.Business.Product", "Product")
                        .WithMany("SaleDetailsTemp")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_SaleDetailTemp_ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marquesita.Models.Business.ShoppingCart", b =>
                {
                    b.HasOne("Marquesita.Models.Business.Product", "Products")
                        .WithMany("ShopingCartItems")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_ShoppingCart_ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marquesita.Models.Business.WishList", b =>
                {
                    b.HasOne("Marquesita.Models.Business.Product", "Product")
                        .WithMany("WishLists")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_WishList_ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
