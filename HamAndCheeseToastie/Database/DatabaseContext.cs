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

        // Defining DbSets with lowercase table names to match PostgreSQL conventions
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transaction { get; set; } = default!;
        public DbSet<HamAndCheeseToastie.Models.TransactionItem> TransactionItem { get; set; } = default!;
        public DbSet<HamAndCheeseToastie.Models.Customer> Customer { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<Transaction>().ToTable("transaction");
            modelBuilder.Entity<TransactionItem>().ToTable("transaction_item");
            modelBuilder.Entity<Customer>().ToTable("customer");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.PaymentMethod)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(p => p.CategoryId)
                .HasColumnName("category_id");

            modelBuilder.Entity<TransactionItem>()
        .Property(ti => ti.ProductId)
        .HasColumnName("productid");

            base.OnModelCreating(modelBuilder);
        }
    }
}


