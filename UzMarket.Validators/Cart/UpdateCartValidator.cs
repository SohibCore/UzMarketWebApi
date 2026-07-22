using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.CartDtos;

namespace UzMarket.Validators.Cart
{
    public class UpdateCartValidator : AbstractValidator<UpdateCartDlDto>
    {
        public UpdateCartValidator(IValidator<UpdateCartItemDlDto> validator)
        {
            RuleFor(x => x.StatusId)
                .NotEmpty().WithMessage("The OrderStatusId field is required.")
                .When(x => x.StatusId != null);

            RuleForEach(x => x.Tables)
                .SetValidator(validator)
                .When(x => x.Tables != null);
        }
    }
}
