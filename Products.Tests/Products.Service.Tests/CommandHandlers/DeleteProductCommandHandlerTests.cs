using FluentAssertions;
using Moq;
using Products.Repository.Interfaces;
using Products.Service.CommandHandlers;
using Products.Service.Commands;

namespace Products.Tests.Products.Service.Tests.CommandHandlers
{
    public class DeleteProductCommandHandlerTests
    {
        [Fact]
        public async Task GivenProductId_WhenHandle_ThenDeletesProduct()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);
            var handler = new DeleteProductCommandHandler(repoMock.Object);
            var command = new DeleteProductCommand { Id = 1 };

            // When
            var result = await handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().Be(1);
            repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}