using FluentAssertions;
using FluentValidation.TestHelper;
using Products.WebAPI.Validators;
using Xunit;

namespace Products.Tests.Products.WebAPI.Tests.Validators
{
    public class StockValidatorTests
    {
        private readonly StockValidator _validator = new StockValidator();

        [Fact]
        public void GivenNegativeQuantity_WhenValidate_ThenValidationFails()
        {
            // Given
            int quantity = -1;

            // When
            var result = _validator.TestValidate(quantity);

            // Then
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(q => q)
                .WithErrorMessage("Quantity must be non-negative.");
        }

        [Fact]
        public void GivenZeroQuantity_WhenValidate_ThenValidationSucceeds()
        {
            // Given
            int quantity = 0;

            // When
            var result = _validator.TestValidate(quantity);

            // Then
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void GivenPositiveQuantity_WhenValidate_ThenValidationSucceeds()
        {
            // Given
            int quantity = 5;

            // When
            var result = _validator.TestValidate(quantity);

            // Then
            result.IsValid.Should().BeTrue();
        }
    }
}