using HamAndCheeseToastie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using NpgsqlTypes;

namespace HamAndCheeseToastie.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // DbSet definitions
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transaction { get; set; } = default!;
        public DbSet<TransactionItem> TransactionItem { get; set; } = default!;
        public DbSet<Customer> Customer { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // DateTime UTC conversion for all DateTime properties
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                    }
                }
            }

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.HasIndex(e => e.email).IsUnique();
                entity.Property(e => e.username).HasColumnName("username");
                entity.Property(e => e.email).HasColumnName("email");
                entity.Property(e => e.password_hash).HasColumnName("passwordhash");
                entity.Property(e => e.created_at)
                    .HasColumnName("createdat")
                    .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                entity.Property(e => e.updated_at)
                    .HasColumnName("updatedat")
                    .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                entity.Property(e => e.Role).HasColumnName("role");
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transactionid")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transactiondate");

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("totalamount");

                entity.Property(e => e.Discount)
                    .HasColumnName("discount");

                entity.Property(e => e.PaymentMethod)
                    .HasColumnName("paymentmethod")
                    .HasConversion<string>();

                entity.Property(e => e.TaxAmount)
                    .HasColumnName("taxamount");

                entity.Property(e => e.UserId)
                    .HasColumnName("userid");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerid");

                // Configure relationship with Customer
                entity.HasOne(t => t.Customer)
                    .WithMany()
                    .HasForeignKey(t => t.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TransactionItem configuration
            modelBuilder.Entity<TransactionItem>(entity =>
            {
                entity.ToTable("transaction_item");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transactionid");

                entity.Property(e => e.ProductId)
                    .HasColumnName("productid");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity");

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("unitprice");

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("totalprice");

                // Create index on TransactionId for better join performance
                entity.HasIndex(e => e.TransactionId);
            });

            // Other entity configurations
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<Customer>().ToTable("customer");

            // Product CategoryId configuration
            modelBuilder.Entity<Product>()
                .Property(p => p.CategoryId)
                .HasColumnName("category_id");

            base.OnModelCreating(modelBuilder);
        }
    }
}