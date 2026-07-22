using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;

namespace UzMarket.Validators.Product
{
    public class CreateProductImageValidator : AbstractValidator<CreateProductImageDlDto>
    {
        public CreateProductImageValidator()
        {
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("The Image field is required.");

            RuleFor(x => x.SortOrder)
                .GreaterThan(0).WithMessage("The SortOrder field is required.");

            /*RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("The Product field is required.");*/
        }
    }
}
