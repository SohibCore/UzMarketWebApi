using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;

namespace UzMarket.Validators.Product
{
    public class UpdateProductImageValidator : AbstractValidator<UpdateProductImageDlDto>
    {
        public UpdateProductImageValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid ProductImage Id is required to identify which item to update.")
                .When(x => x.Id != null);

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("ImageUrl cannot be empty")
                .When(x => x.ImageUrl != null);

            RuleFor(x => x.SortOrder)
                .GreaterThan(0).WithMessage("The SortOrder field must be a valid product.")
                .When(x => x.SortOrder != null);

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("The ProductId field must be a valid product.")
                .When(x => x.ProductId != null);
        }
    }
}
