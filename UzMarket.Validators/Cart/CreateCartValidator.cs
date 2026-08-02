using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.CartDtos;

namespace UzMarket.Validators
{
    public class CreateCartValidator : AbstractValidator<CreateCartDlDto>
    {
        public CreateCartValidator(IValidator<CreateCartItemDlDto> validator)
        {
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("The Cart items collection must be provided.");

            RuleForEach(x => x.Items).SetValidator(validator);
        }
    }
}
