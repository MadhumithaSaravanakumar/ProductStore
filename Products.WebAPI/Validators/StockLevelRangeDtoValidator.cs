using FluentValidation;
using Products.WebAPI.DTOs;

namespace Products.WebAPI.Validators
{
    public class StockLevelRangeDtoValidator : AbstractValidator<StockLevelRangeDto>
    {
        public StockLevelRangeDtoValidator()
        {
            RuleFor(x => x.Min)
                .GreaterThanOrEqualTo(0).WithMessage("Min value cannot be negative.");
            RuleFor(x => x.Max)
                .GreaterThanOrEqualTo(0).WithMessage("Max value cannot be negative.");
            RuleFor(x => x)
                .Must(x => x.Min <= x.Max)
                .WithMessage("Min must be less than or equal to Max.");
        }
    }
}
