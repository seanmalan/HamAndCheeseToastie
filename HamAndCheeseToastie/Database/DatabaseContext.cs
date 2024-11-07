using HamAndCheeseToastie.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using NpgsqlTypes;


namespace HamAndCheeseToastie.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HamAndCheeseToastie.Models.Transaction> Transaction { get; set; } = default!;
        public DbSet<HamAndCheeseToastie.Models.TransactionItem> TransactionItem { get; set; } = default!;
        public DbSet<HamAndCheeseToastie.Models.Cashier> Cashier { get; set; } = default!;
        public DbSet<HamAndCheeseToastie.Models.Customer> Customer { get; set; } = default!;

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


    }
}
