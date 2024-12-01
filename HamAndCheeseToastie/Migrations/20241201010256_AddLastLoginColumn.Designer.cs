﻿// <auto-generated />
using System;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20241201010256_AddLastLoginColumn")]
    partial class AddLastLoginColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HamAndCheeseToastie.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("CategoryId");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("HamAndCheeseToastie.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("customerid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CustomerId"));

                    b.Property<string>("Barcode")
                        .HasColumnType("text")
                        .HasColumnName("barcode");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstname");

                    b.Property<bool>("IsLoyaltyMember")
                        .HasColumnType("boolean")
                        .HasColumnName("isloyaltymember");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastname");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phonenumber");

                    b.HasKey("CustomerId");

                    b.ToTable("customer", (string)null);
                });

            modelBuilder.Entity("HamAndCheeseToastie.Models.InventoryLog", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LogID"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("barcode");

                    b.Property<string>("BrandNewValue")
                        .HasColumnType("text")
                        .HasColumnName("brand_new_value");

                    b.Property<string>("BrandOldValue")
                        .HasColumnType("text")
                        .HasColumnName("brand_old_value");

                    b.Property<string>("CategoryNewValue")
                        .HasColumnType("text")
                        .HasColumnName("category_new_value");

                    b.Property<string>("CategoryOldValue")
                        .HasColumnType("text")
                        .HasColumnName("category_old_value");

                    b.Property<string>("ChangeType")
                        .HasColumnType("text")
                        .HasColumnName("change_type");

                    b.Property<string>("NameNewValue")
                        .HasColumnType("text")
                        .HasColumnName("name_new_value");

                    b.Property<string>("NameOldValue")
                        .HasColumnType("text")
                        .HasColumnName("name_old_value");

                    b.Property<decimal>("PriceNewValue")
                        .HasColumnType("numeric")
                        .HasColumnName("price_new_value");

                    b.Property<decimal>("PriceOldValue")
                        .HasColumnType("numeric")
                        .HasColumnName("price_old_value");

                    b.Property<int>("ProductID")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<string>("ReductionReason")
                        .HasColumnType("text")
                        .HasColumnName("reduction_reason");

                    b.Property<string>("StockNewValue")
                        .HasColumnType("text")
                        .HasColumnName("stock_new_value");

                    b.Property<string>("StockOldValue")
                        .HasColumnType("text")
                        .HasColumnName("stock_old_value");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("timestamp");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<decimal>("WholesalePriceNewValue")
                        .HasColumnType("numeric")
                        .HasColumnName("wholesale_price_new_value");

                    b.Property<decimal>("WholesalePriceOldValue")
                        .HasColumnType("numeric")
                        .HasColumnName("wholesale_price_old_value");

                    b.HasKey("LogID");

                    b.HasIndex("ProductID");

                    b.ToTable("inventory_logs");
                });

            modelBuilder.Entity("HamAndCheeseToastie.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("HamAndCheeseToastie.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("Passcode")
                        .HasColumnType("text");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PasswordResetTokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTime?>("last_login_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("password_hash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passwordhash");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updatedat");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("id");

                    b.HasIndex("email")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("brand_name");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<int?>("CategoryId1")
                        .HasColumnType("integer");

                    b.Property<int>("CurrentStockLevel")
                        .HasColumnType("integer")
                        .HasColumnName("current_stock_level");

                    b.Property<string>("EAN13Barcode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ean13_barcode");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_path");

                    b.Property<int>("MinimumStockLevel")
                        .HasColumnType("integer")
                        .HasColumnName("minimum_stock_level");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("Weight")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("weight");

                    b.Property<decimal>("WholesalePrice")
                        .HasColumnType("numeric")
                        .HasColumnName("wholesale_price");

                    b.HasKey("ID");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CategoryId1");

                    b.HasIndex("EAN13Barcode")
                        .IsUnique();

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("transactionid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionId"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer")
                        .HasColumnName("customerid");

                    b.Property<decimal>("Discount")
                        .HasColumnType("numeric")
                        .HasColumnName("discount");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("paymentmethod");

                    b.Property<decimal>("TaxAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("taxamount");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("totalamount");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("transactiondate");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userid");

                    b.HasKey("TransactionId");

                    b.HasIndex("CustomerId");

                    b.ToTable("transaction", (string)null);
                });

            modelBuilder.Entity("TransactionItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("productid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("totalprice");

                    b.Property<int>("TransactionId")
                        .HasColumnType("integer")
                        .HasColumnName("transactionid");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric")
                        .HasColumnName("unitprice");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("transaction_item", (string)null);
                });

            modelBuilder.Entity("HamAndCheeseToastie.Models.InventoryLog", b =>
                {
                    b.HasOne("Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Product", b =>
                {
                    b.HasOne("HamAndCheeseToastie.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HamAndCheeseToastie.Models.Category", null)
                        .WithMany("Products")
                        .HasForeignKey("CategoryId1");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Transaction", b =>
                {
                    b.HasOne("HamAndCheeseToastie.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("HamAndCheeseToastie.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
