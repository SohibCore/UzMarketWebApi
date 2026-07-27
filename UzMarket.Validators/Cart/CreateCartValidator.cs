using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.CartDtos;
using UzMarket.Validators.Cart;

namespace UzMarket.Validators
{
    public class CreateCartValidator : AbstractValidator<CreateCartDlDto>
    {
        public CreateCartValidator(IValidator<CreateCartItemDlDto> validator)
        {
            RuleFor(x => x.Items)
                .NotNull().WithMessage("The Cart items collection must be provided.");

            RuleForEach(x => x.Items).SetValidator(validator);
        }
    }
}
