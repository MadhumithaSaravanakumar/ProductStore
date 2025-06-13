using FluentValidation.TestHelper;
using Products.WebAPI.DTOs;
using Products.WebAPI.Validators;

namespace Products.Tests.Products.WebAPI.Tests.Validators
{
    public class StockLevelRangeDtoValidatorTests
    {
        private readonly StockLevelRangeDtoValidator _validator;

        public StockLevelRangeDtoValidatorTests()
        {
            _validator = new StockLevelRangeDtoValidator();
        }

        [Fact(DisplayName = "Given Min is negative, When validating, Then validation error for Min")]
        public void Given_Min_Is_Negative_When_Validating_Then_Validation_Error_For_Min()
        {
            // Given
            var dto = new StockLevelRangeDto { Min = -1, Max = 10 };

            // When
            var result = _validator.TestValidate(dto);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Min)
                .WithErrorMessage("Min value cannot be negative.");
        }

        [Fact(DisplayName = "Given Max is negative, When validating, Then validation error for Max")]
        public void Given_Max_Is_Negative_When_Validating_Then_Validation_Error_For_Max()
        {
            // Given
            var dto = new StockLevelRangeDto { Min = 5, Max = -1 };

            // When
            var result = _validator.TestValidate(dto);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Max)
                .WithErrorMessage("Max value cannot be negative.");
        }

        [Fact(DisplayName = "Given Min is greater than Max, When validating, Then validation error for Min <= Max")]
        public void Given_Min_Greater_Than_Max_When_Validating_Then_Validation_Error_For_Min_Less_Than_Or_Equal_Max()
        {
            // Given
            var dto = new StockLevelRangeDto { Min = 20, Max = 10 };

            // When
            var result = _validator.TestValidate(dto);

            // Then
            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage("Min must be less than or equal to Max.");
        }

        [Fact(DisplayName = "Given valid Min and Max, When validating, Then no validation errors")]
        public void Given_Valid_Min_And_Max_When_Validating_Then_No_Validation_Errors()
        {
            // Given
            var dto = new StockLevelRangeDto { Min = 5, Max = 10 };

            // When
            var result = _validator.TestValidate(dto);

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}