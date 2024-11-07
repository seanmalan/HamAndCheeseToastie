using HamAndCheeseToastie.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using NpgsqlTypes;


namespace HamAndCheeseToastie.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // Defining DbSets with lowercase table names to match PostgreSQL conventions
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; } = default!;
        public DbSet<TransactionItem> TransactionItems { get; set; } = default!;
        public DbSet<Cashier> Cashiers { get; set; } = default!;
        public DbSet<Customer> Customers { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // Explicitly map to "Users" table with double quotes for PostgreSQL
                entity.ToTable("Users");

                // Map property names to exact column names
                entity.Property(e => e.id).HasColumnName("Id");
                entity.Property(e => e.username).HasColumnName("Username");
                entity.Property(e => e.email).HasColumnName("Email");
                entity.Property(e => e.password_hash).HasColumnName("PasswordHash");
                entity.Property(e => e.created_at).HasColumnName("CreatedAt");
                entity.Property(e => e.updated_at).HasColumnName("UpdatedAt");
                entity.Property(e => e.roleId).HasColumnName("Role");
            });

            modelBuilder.Entity<Transaction>()
        .Property(t => t.PaymentMethod)
        .HasConversion<string>();

            // Additional configurations if needed
            base.OnModelCreating(modelBuilder);
        }

            // Set table names explicitly in lowercase to match PostgreSQL conventions
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<Transaction>().ToTable("transactions");
            modelBuilder.Entity<TransactionItem>().ToTable("transaction_items");
            modelBuilder.Entity<Cashier>().ToTable("cashiers");
            modelBuilder.Entity<Customer>().ToTable("customers");
        }
    }
}
