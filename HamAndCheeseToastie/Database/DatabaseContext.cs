using HamAndCheeseToastie.Models;
using Microsoft.EntityFrameworkCore;

namespace HamAndCheeseToastie.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HamAndCheeseToastie.Models.Transaction> Transaction { get; set; } = default!;
        public DbSet<HamAndCheeseToastie.Models.TransactionItem> TransactionItem { get; set; } = default!;
        public DbSet<HamAndCheeseToastie.Models.Cashier> Cashier { get; set; } = default!;
        public DbSet<HamAndCheeseToastie.Models.Customer> Customer { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify table name for User entity
            modelBuilder.Entity<User>().ToTable("users"); // specify the table name in lowercase

            // You can add other entity configurations here as needed
            base.OnModelCreating(modelBuilder);
        }

    }
}
