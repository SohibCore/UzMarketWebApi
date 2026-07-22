using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;

namespace UzMarket.Validators.Product
{
    public class CreateProductValidator : AbstractValidator<CreateProductDlDto>
    {
        public CreateProductValidator(IValidator<CreateProductImageDlDto> validator)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The Name field is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The Description field is required.");

            RuleFor(x => x.Price)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The Price field must be a valid price.")
                .PrecisionScale(18, 2, ignoreTrailingZeros: true)
                .WithMessage("Price can have at most 2 decimal places.");

            RuleFor(x => x.StockQuantity)
                .GreaterThan(0).WithMessage("The StockQuantity field must be a valid.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("The CategoryId field must be a valid.");

            RuleFor(x => x.Tables)
                .NotEmpty().WithMessage("The Product must contain at least one item.");

            RuleForEach(x => x.Tables)
                .SetValidator(validator);
        }
    }
}
