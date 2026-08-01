using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;

namespace UzMarket.Validators.Product
{
    public class UpdateProductImageValidator : AbstractValidator<UpdateProductImageDlDto>
    {
        public UpdateProductImageValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid ProductImage Id is required to identify which item to update.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("ImageUrl cannot be empty")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The ImageUrl field cannot be empty.")
                .When(x => x.ImageUrl != null);

            RuleFor(x => x.SortOrder)
                .GreaterThan(0).WithMessage("The SortOrder field must be a valid product.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("The ProductId field must be a valid product.");
        }
    }
}
