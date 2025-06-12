using FluentAssertions;
using Moq;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.QueryHandlers;
using Products.Service.Queries;

namespace Products.Tests.Products.Service.Tests.QueryHandlers
{
    public class GetProductsByStockLevelQueryHandlerTests
    {
        [Fact]
        public async Task GivenProducts_WhenHandle_ThenReturnsProductsWithinStockRange()
        {
            // Given
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "A", Stock = 5 },
                new Product { Id = 2, Name = "B", Stock = 15 },
                new Product { Id = 3, Name = "C", Stock = 25 }
            };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            var handler = new GetProductsByStockLevelQueryHandler(repoMock.Object);
            var query = new GetProductsByStockLevelQuery { Min = 10, Max = 20 };

            // When
            var result = await handler.Handle(query, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(products.Where(p => p.Stock >= 10 && p.Stock <= 20));
        }
    }
}