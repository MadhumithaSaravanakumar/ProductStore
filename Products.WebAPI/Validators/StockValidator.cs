using FluentValidation;
using Products.WebAPI.DTOs;

namespace Products.WebAPI.Validators
{
    public class StockValidator : AbstractValidator<int>
    {
        public StockValidator()
        {
            RuleFor(q => q)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity must be non-negative.");
        }
    }
}