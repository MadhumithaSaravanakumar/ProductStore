using Microsoft.EntityFrameworkCore;
using Products.Common.Entities;

namespace Products.Repository.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Product>()
                .Property(p => p.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 100000, Name = "Sample Product", Description = "Sample Description", Stock = 10 },
                new Product { Id = 100001, Name = "Another Product", Description = "Another Description", Stock = 20 }
            );
        }
    }
}
