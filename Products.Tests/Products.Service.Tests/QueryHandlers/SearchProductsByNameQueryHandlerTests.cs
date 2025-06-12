using FluentAssertions;
using Moq;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.QueryHandlers;
using Products.Service.Queries;

namespace Products.Tests.Products.Service.Tests.QueryHandlers
{
    public class SearchProductsByNameQueryHandlerTests
    {
        [Fact]
        public async Task GivenProducts_WhenHandle_ThenReturnsProductsMatchingName()
        {
            // Given
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Apple", Stock = 5 },
                new Product { Id = 2, Name = "Banana", Stock = 10 },
                new Product { Id = 3, Name = "Pineapple", Stock = 15 }
            };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            var handler = new SearchProductsByNameQueryHandler(repoMock.Object);
            var query = new SearchProductsByNameQuery { Name = "apple" };

            // When
            var result = await handler.Handle(query, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(products.Where(p => p.Name.Contains("apple", System.StringComparison.OrdinalIgnoreCase)));
        }
    }
}