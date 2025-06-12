using FluentAssertions;
using Moq;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.QueryHandlers;
using Products.Service.Queries;

namespace Products.Tests.Products.Service.Tests.QueryHandlers
{
    public class GetAllProductsQueryHandlerTests
    {
        [Fact]
        public async Task GivenRepositoryWithProducts_WhenHandle_ThenReturnsAllProducts()
        {
            // Given
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "A", Stock = 10 },
                new Product { Id = 2, Name = "B", Stock = 20 }
            };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            var handler = new GetAllProductsQueryHandler(repoMock.Object);
            var query = new GetAllProductsQuery();

            // When
            var result = await handler.Handle(query, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(products);
        }
    }
}