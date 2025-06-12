using FluentAssertions;
using Moq;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.CommandHandlers;
using Products.Service.Commands;


namespace Products.Tests.Products.Service.Tests.CommandHandlers
{
    public class AddProductCommandHandlerTests
    {
        [Fact]
        public async Task GivenValidProduct_WhenHandle_ThenAssignsUniqueIdAndAddsProduct()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            var product = new Product { Name = "Test", Description = "Desc", Stock = 1 };
            var command = new AddProductCommand { Product = product };
            repoMock.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(42);
            // UniqueIdentifierGenerator is static, so we can't mock it directly, but we can check that Id is set
            repoMock.Setup(r => r.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var handler = new AddProductCommandHandler(repoMock.Object);

            // When
            var result = await handler.Handle(command, CancellationToken.None);

            // Then
            product.Id.Should().BeGreaterThanOrEqualTo(100000);
            repoMock.Verify(r => r.AddAsync(It.Is<Product>(p => p.Id == product.Id)), Times.Once);
            result.Should().Be(42);
        }
    }
}