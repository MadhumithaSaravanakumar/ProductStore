using FluentAssertions;
using FluentValidation.TestHelper;
using Products.WebAPI.DTOs;
using Products.WebAPI.Validators;
using Xunit;

namespace Products.Tests.Products.WebAPI.Tests.Validators
{
    public class ProductDtoValidatorTests
    {
        private readonly ProductDtoValidator _validator = new ProductDtoValidator();

        [Fact]
        public void GivenEmptyName_WhenValidate_ThenValidationFails()
        {
            // Given
            var dto = new ProductDto { Name = "", Description = "desc", Stock = 1 };

            // When
            var result = _validator.TestValidate(dto);

            // Then
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name is required.");
        }

        [Fact]
        public void GivenLongName_WhenValidate_ThenValidationFails()
        {
            // Given
            var dto = new ProductDto { Name = new string('a', 51), Description = "desc", Stock = 1 };

            // When
            var result = _validator.TestValidate(dto);

            // Then
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name cannot exceed 50 characters.");
        }

        [Fact]
        public void GivenLongDescription_WhenValidate_ThenValidationFails()
        {
            // Given
            var dto = new ProductDto { Name = "Valid", Description = new string('b', 101), Stock = 1 };

            // When
            var result = _validator.TestValidate(dto);

            // Then
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Description cannot exceed 100 characters.");
        }

        [Fact]
        public void GivenNegativeStock_WhenValidate_ThenValidationFails()
        {
            // Given
            var dto = new ProductDto { Name = "Valid", Description = "desc", Stock = -1 };

            // When
            var result = _validator.TestValidate(dto);

            // Then
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.Stock)
                .WithErrorMessage("Stock must be non-negative.");
        }

        [Fact]
        public void GivenValidProductDto_WhenValidate_ThenValidationSucceeds()
        {
            // Given
            var dto = new ProductDto { Name = "Valid", Description = "desc", Stock = 10 };

            // When
            var result = _validator.TestValidate(dto);

            // Then
            result.IsValid.Should().BeTrue();
        }
    }
}