using FluentValidation;
using Products.WebAPI.DTOs;

namespace Products.WebAPI.Validators
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be non-negative.");
        }
    }
}