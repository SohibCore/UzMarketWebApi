using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;

namespace UzMarket.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDlDto>
    {
        public CreateOrderValidator(IValidator<CreateOrderItemDlDto> validator)
        {
            RuleFor(x => x.ShippingAddressId)
                .GreaterThan(0).WithMessage("The ShippingAddressId field must be a valid address.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("The order must contain at least one item.");

            RuleForEach(x => x.Items).SetValidator(validator);
        }
    }
}
