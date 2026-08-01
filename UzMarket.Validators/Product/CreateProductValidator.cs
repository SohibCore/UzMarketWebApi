using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;

namespace UzMarket.Validators.Product
{
    public class CreateProductValidator : AbstractValidator<CreateProductDlDto>
    {
        public CreateProductValidator(IValidator<CreateProductImageDlDto> validator)
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The Name field is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Name field cannot be empty.")
                .MinimumLength(3).WithMessage("The Name must be at least 3 character.")
                .MaximumLength(200).WithMessage("The Name must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Description field cannot be empty.")
                .MinimumLength(3).WithMessage("The Name must be at least 3 character.")
                .MaximumLength(1000).WithMessage("The Name must not exceed 1000 characters.");

            RuleFor(x => x.Price)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The Price field must be a valid price.")
                .PrecisionScale(18, 2, ignoreTrailingZeros: true)
                .WithMessage("Price can have at most 2 decimal places.");

            RuleFor(x => x.StockQuantity)
                .GreaterThan(0).WithMessage("The StockQuantity field must be a valid.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("The Category field must be a valid.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("The Product must contain at least one item.");

            RuleForEach(x => x.Items)
                .SetValidator(validator);
        }
    }
}
