using FluentAssertions;
using Moq;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.CommandHandlers;
using Products.Service.Commands;
using Products.Service.CommandResults;

namespace Products.Tests.Products.Service.Tests.CommandHandlers
{
    public class DecrementStockCommandHandlerTests
    {
        [Fact]
        public async Task GivenProductNotFound_WhenHandle_ThenReturnsNotFoundResult()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);
            var handler = new DecrementStockCommandHandler(repoMock.Object);
            var command = new DecrementStockCommand { Id = 1, Quantity = 2 };

            // When
            var result = await handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(DecrementStockResult.NotFoundResult());
        }

        [Fact]
        public async Task GivenInsufficientStock_WhenHandle_ThenReturnsNotFoundResult()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            var product = new Product { Id = 1, Name = "Sample", Stock = 1 };
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            var handler = new DecrementStockCommandHandler(repoMock.Object);
            var command = new DecrementStockCommand { Id = 1, Quantity = 2 };

            // When
            var result = await handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(DecrementStockResult.StockUnavailableResult(product.Stock));
        }

        [Fact]
        public async Task GivenSufficientStock_WhenHandle_ThenDecrementsStockAndReturnsSuccess()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            var product = new Product { Id = 1, Name = "Sample", Stock = 5 };
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            repoMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(1);
            var handler = new DecrementStockCommandHandler(repoMock.Object);
            var command = new DecrementStockCommand { Id = 1, Quantity = 2 };

            // When
            var result = await handler.Handle(command, CancellationToken.None);

            // Then
            product.Stock.Should().Be(3);
            result.Should().BeEquivalentTo(DecrementStockResult.SuccessResult(3));
            repoMock.Verify(r => r.UpdateAsync(product), Times.Once);
        }
    }
}
