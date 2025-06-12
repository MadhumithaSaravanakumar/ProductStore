using FluentAssertions;
using Moq;
using Products.Repository.Interfaces;
using Products.Utility.Generator;

namespace Products.Tests.Products.Utility.Tests.Generator
{
    public class UniqueIdentifierGeneratorTests
    {
        [Fact]
        public async Task GivenRepository_WhenGenerateUniqueIdAsync_ThenReturnsUniqueId()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // When
            var id = await UniqueIdentifierGenerator.GenerateUniqueIdAsync(repoMock.Object);

            // Then
            id.Should().BeGreaterThanOrEqualTo(100000);
            id.Should().BeLessThan(1000000);
        }

        [Fact]
        public async Task GivenRepositoryWithExistingIds_WhenGenerateUniqueIdAsync_ThenRetriesUntilUnique()
        {
            // Given
            var repoMock = new Mock<IProductRepository>();
            int callCount = 0;
            repoMock.Setup(r => r.ExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => callCount++ < 2);

            // When
            var id = await UniqueIdentifierGenerator.GenerateUniqueIdAsync(repoMock.Object);

            // Then
            callCount.Should().Be(3);
            id.Should().BeGreaterThanOrEqualTo(100000);
            id.Should().BeLessThan(1000000);
        }
    }
}