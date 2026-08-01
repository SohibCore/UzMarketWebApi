using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;

namespace UzMarket.Validators.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDlDto>
    {
        public UpdateProductValidator(IValidator<UpdateProductImageDlDto> validator)
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The Name field is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Name field cannot be empty.")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Description field cannot be empty.")
                .MinimumLength(3).WithMessage("The Name must be at least 3 character.")
                .MaximumLength(1000).WithMessage("The Name must not exceed 1000 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.Price)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The Price field must be a valid quantity.")
                .PrecisionScale(18, 2, ignoreTrailingZeros: true)
                .WithMessage("Price can have at most 2 decimal places.")
                .When(x => x.Price != null);

            RuleFor(x => x.StockQuantity)
                .GreaterThan(0).WithMessage("The StockQuantity field must be a valid.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Please enter a valid number for Category.");

            RuleFor(x => x.SupplierId)
                .GreaterThan(0).WithMessage("Please enter a valid number for Supplier.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("The Product must contain at least one item.");

            RuleForEach(x => x.Items)
                .SetValidator(validator);
        }
    }
}
