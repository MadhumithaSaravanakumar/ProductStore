using Microsoft.EntityFrameworkCore;
using Products.Common.Entities;
using Products.Repository;
using Products.Repository.Data;
using Products.Utility.Exceptions;

namespace Products.Tests.Products.RepositoryTests
{
    public class ProductRepositoryTests
    {
        private ProductDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new ProductDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var context = CreateContext(nameof(GetAllAsync_ShouldReturnAllProducts));
            context.Products.AddRange(
                new Product { Id = 1, Name = "Product 1", Stock = 2 },
                new Product { Id = 2, Name = "Product 2", Stock = 1 }
            );
            context.SaveChanges();
            var repo = new ProductRepository(context);

            // Act
            var result = await repo.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ProductExists_ShouldReturnProduct()
        {
            // Arrange
            var context = CreateContext(nameof(GetByIdAsync_ProductExists_ShouldReturnProduct));
            var product = new Product { Id = 1, Name = "Product 1", Stock = 1 };
            context.Products.Add(product);
            context.SaveChanges();
            var repo = new ProductRepository(context);

            // Act
            var result = await repo.GetByIdAsync(1);

            // Assert
            Assert.Equal(product.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ProductDoesNotExist_ShouldThrowRepositoryException()
        {
            // Arrange
            var context = CreateContext(nameof(GetByIdAsync_ProductDoesNotExist_ShouldThrowRepositoryException));
            var repo = new ProductRepository(context);

            // Act & Assert
            await Assert.ThrowsAsync<RepositoryException>(() => repo.GetByIdAsync(1));
        }

        [Fact]
        public async Task AddAsync_ShouldAddProductAndReturnId()
        {
            // Arrange
            var context = CreateContext(nameof(AddAsync_ShouldAddProductAndReturnId));
            var product = new Product { Id = 1, Name = "Product 1", Stock = 2 };
            var repo = new ProductRepository(context);

            // Act
            var result = await repo.AddAsync(product);

            // Assert
            Assert.Equal(product.Id, result);
            Assert.NotNull(context.Products.Find(product.Id));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProductAndReturnId()
        {
            // Arrange
            var context = CreateContext(nameof(UpdateAsync_ShouldUpdateProductAndReturnId));
            var product = new Product { Id = 1, Name = "Old Name", Stock = 2 };
            context.Products.Add(product);
            context.SaveChanges();
            var repo = new ProductRepository(context);

            // Act
            product.Name = "Updated Product";
            var result = await repo.UpdateAsync(product);

            // Assert
            Assert.Equal(product.Id, result);
            Assert.Equal("Updated Product", context.Products.Find(product.Id).Name);
        }

        [Fact]
        public async Task DeleteAsync_ProductExists_ShouldReturnDeletedProductId()
        {
            // Arrange
            var context = CreateContext(nameof(DeleteAsync_ProductExists_ShouldReturnDeletedProductId));
            var product = new Product { Id = 1, Name = "Product 1", Stock = 2 };
            context.Products.Add(product);
            context.SaveChanges();
            var repo = new ProductRepository(context);

            // Act
            var result = await repo.DeleteAsync(1);

            // Assert
            Assert.Equal(product.Id, result);
            Assert.Null(context.Products.Find(product.Id));
        }

        [Fact]
        public async Task DeleteAsync_ProductDoesNotExist_ShouldThrowRepositoryException()
        {
            // Arrange
            var context = CreateContext(nameof(DeleteAsync_ProductDoesNotExist_ShouldThrowRepositoryException));
            var repo = new ProductRepository(context);

            // Act & Assert
            await Assert.ThrowsAsync<RepositoryException>(() => repo.DeleteAsync(1));
        }
    }
}