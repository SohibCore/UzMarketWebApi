using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.CartDtos;
using UzMarket.Validators.Cart;

namespace UzMarket.Validators
{
    public class CreateCartValidator : AbstractValidator<CreateCartDlDto>
    {
        public CreateCartValidator(IValidator<CreateCartItemDlDto> validator)
        {
            RuleFor(x => x.Tables)
                .NotNull().WithMessage("The Cart items collection must be provided.");

            RuleForEach(x => x.Tables).SetValidator(validator);
        }
    }
}
