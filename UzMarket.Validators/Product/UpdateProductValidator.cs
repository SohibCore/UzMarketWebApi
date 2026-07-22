using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;

namespace UzMarket.Validators.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDlDto>
    {
        public UpdateProductValidator(IValidator<UpdateProductImageDlDto> validator)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description cannot be empty")
                .When(x => x.Description != null);

            RuleFor(x => x.Price)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The Price field must be a valid quantity.")
                .When(x => x.Price != null);

            RuleFor(x => x.StockQuantity)
                .GreaterThan(0).WithMessage("The StockQuantity field must be a valid.")
                .When(x => x.StockQuantity != null);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Please enter a valid number for Category.")
                .When(x => x.CategoryId != null);

            RuleFor(x => x.SupplierId)
                .GreaterThan(0).WithMessage("Please enter a valid number for Supplier.")
                .When(x => x.SupplierId != null);

            RuleFor(x => x.Tables)
                .NotEmpty().WithMessage("The Product must contain at least one item.");

            RuleForEach(x => x.Tables)
                .SetValidator(validator);
        }
    }
}
