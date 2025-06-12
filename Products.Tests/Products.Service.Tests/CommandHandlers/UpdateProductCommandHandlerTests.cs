using FluentAssertions;
using Moq;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.CommandHandlers;
using Products.Service.Commands;

namespace Products.Tests.Products.Service.Tests.CommandHandlers
{
    public class UpdateProductCommandHandlerTests
    {
        [Fact]
        public async Task GivenProduct_WhenHandle_ThenUpdatesProduct()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            var product = new Product { Id = 1, Name = "Updated", Description = "Desc", Stock = 10 };
            repoMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(1);
            var handler = new UpdateProductCommandHandler(repoMock.Object);
            var command = new UpdateProductCommand { Product = product };

            // When
            var result = await handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().Be(1);
            repoMock.Verify(r => r.UpdateAsync(product), Times.Once);
        }
    }
}