using HamAndCheeseToastie.Models;
using Microsoft.EntityFrameworkCore;

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
            base.OnModelCreating(modelBuilder);

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
