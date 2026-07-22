using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.CartDtos;

namespace UzMarket.Validators.Cart
{
    public class CreateCartItemValidator : AbstractValidator<CreateCartItemDlDto>
    {
        public CreateCartItemValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
