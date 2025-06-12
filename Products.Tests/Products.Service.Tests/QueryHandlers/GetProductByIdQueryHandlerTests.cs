using FluentAssertions;
using Moq;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.QueryHandlers;
using Products.Service.Queries;

namespace Products.Tests.Products.Service.Tests.QueryHandlers
{
    public class GetProductByIdQueryHandlerTests
    {
        [Fact]
        public async Task GivenProductExists_WhenHandle_ThenReturnsProduct()
        {
            // Given
            var product = new Product { Id = 1, Name = "Test", Stock = 5 };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            var handler = new GetProductByIdQueryHandler(repoMock.Object);
            var query = new GetProductByIdQuery { Id = 1 };

            // When
            var result = await handler.Handle(query, CancellationToken.None);

            // Then
            result.Should().Be(product);
        }

        [Fact]
        public async Task GivenProductDoesNotExist_WhenHandle_ThenReturnsNull()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Product)null);
            var handler = new GetProductByIdQueryHandler(repoMock.Object);
            var query = new GetProductByIdQuery { Id = 2 };

            // When
            var result = await handler.Handle(query, CancellationToken.None);

            // Then
            result.Should().BeNull();
        }
    }
}