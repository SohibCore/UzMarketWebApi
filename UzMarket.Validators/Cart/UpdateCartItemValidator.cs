using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.CartDtos;

namespace UzMarket.Validators.Cart
{
    public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemDlDto>
    {
        public UpdateCartItemValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .When(x => x.Id.HasValue)
                .WithMessage("A valid CartItem Id is required to identify which item to update.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("The ProductId field must be a valid product.")
                .When(x => x.ProductId != null);

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("The Quantity field must be a valid quantity.")
                .When(x => x.Quantity != null);
        }
    }
}
