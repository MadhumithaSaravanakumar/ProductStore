using FluentAssertions;
using Products.Common.Entities;
using Products.WebAPI.DTOs;
using Products.WebAPI.Mappers;
using Xunit;

namespace Products.Tests.Products.WebAPI.Tests.Mappers
{
    public class ProductMapperTests
    {
        [Fact]
        public void GivenProduct_WhenToResponseDto_ThenMapsAllFields()
        {
            // Given
            var product = new Product
            {
                Id = 10,
                Name = "Test Product",
                Description = "Test Desc",
                Stock = 5
            };

            // When
            var dto = ProductMapper.ToResponseDto(product);

            // Then
            dto.Id.Should().Be(10);
            dto.Name.Should().Be("Test Product");
            dto.Description.Should().Be("Test Desc");
            dto.Stock.Should().Be(5);
        }

        [Fact]
        public void GivenProductWithNullDescription_WhenToResponseDto_ThenDescriptionIsEmptyString()
        {
            // Given
            var product = new Product
            {
                Id = 1,
                Name = "NoDesc",
                Description = null,
                Stock = 2
            };

            // When
            var dto = ProductMapper.ToResponseDto(product);

            // Then
            dto.Description.Should().BeEmpty();
        }

        [Fact]
        public void GivenProductDto_WhenToDomain_ThenMapsAllFields()
        {
            // Given
            var dto = new ProductDto
            {
                Name = "Domain Product",
                Description = "Domain Desc",
                Stock = 7
            };

            // When
            var product = ProductMapper.ToDomain(dto);

            // Then
            product.Name.Should().Be("Domain Product");
            product.Description.Should().Be("Domain Desc");
            product.Stock.Should().Be(7);
        }
    }
}
