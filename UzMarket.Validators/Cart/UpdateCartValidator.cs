using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.CartDtos;

namespace UzMarket.Validators.Cart
{
    public class UpdateCartValidator : AbstractValidator<UpdateCartDlDto>
    {
        public UpdateCartValidator(IValidator<UpdateCartItemDlDto> validator)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required");

            RuleFor(x => x.StatusId)
                .NotEmpty().WithMessage("The OrderStatusId field is required.")
                .When(x => x.StatusId != null);


            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("The Cart must contain at least one item.")
                .When(x => x.Items != null);

            RuleForEach(x => x.Items).SetValidator(validator);
        }
    }
}
