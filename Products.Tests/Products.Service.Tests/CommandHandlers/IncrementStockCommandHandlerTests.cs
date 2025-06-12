using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Products.Common.Entities;
using Products.Repository.Interfaces;
using Products.Service.CommandHandlers;
using Products.Service.Commands;
using Products.Service.CommandResults;
using Xunit;

namespace Products.Tests.Products.Service.Tests.CommandHandlers
{
    public class IncrementStockCommandHandlerTests
    {
        [Fact]
        public async Task GivenProductNotFound_WhenHandle_ThenReturnsNotFoundResult()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);
            var handler = new IncrementStockCommandHandler(repoMock.Object);
            var command = new IncrementStockCommand { Id = 1, Quantity = 2 };

            // When
            var result = await handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(IncrementStockResult.NotFoundResult());
        }

        [Fact]
        public async Task GivenInsufficientStock_WhenHandle_ThenReturnsNotFoundResult()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            var product = new Product { Id = 1, Name="Sample", Stock = 1 };
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            var handler = new IncrementStockCommandHandler(repoMock.Object);
            var command = new IncrementStockCommand { Id = 1, Quantity = 2 };

            // When
            var result = await handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().BeEquivalentTo(IncrementStockResult.NotFoundResult());
        }

        [Fact]
        public async Task GivenSufficientStock_WhenHandle_ThenIncrementsStockAndReturnsSuccess()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            var product = new Product { Id = 1, Name = "Sample", Stock = 5 };
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            repoMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(1);
            var handler = new IncrementStockCommandHandler(repoMock.Object);
            var command = new IncrementStockCommand { Id = 1, Quantity = 2 };

            // When
            var result = await handler.Handle(command, CancellationToken.None);

            // Then
            product.Stock.Should().Be(7);
            result.Should().BeEquivalentTo(IncrementStockResult.SuccessResult(7));
            repoMock.Verify(r => r.UpdateAsync(product), Times.Once);
        }
    }
}