using FluentValidation;

namespace Products.WebAPI.Validators
{
    public class StockValidator : AbstractValidator<int>
    {
        public StockValidator()
        {
            RuleFor(q => q)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        }
    }
}