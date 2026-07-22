using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;

namespace UzMarket.Validators
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderDlDto>
    {
        public UpdateOrderValidator(IValidator<UpdateOrderItemDlDto> validator)
        {
            RuleFor(x => x.OrderStatusId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The OrderStatusId field is required.")
                .When(x => x.OrderStatusId != null);

            RuleFor(x => x.ShippingAddressId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The ShippingAddressId field must be a valid address.")
                .When(x => x.ShippingAddressId != null);

            RuleFor(x => x.Tables)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The order must contain at least one item.")
                .When(x => x.Tables != null);

            RuleForEach(x => x.Tables).SetValidator(validator);
        }
    }
}
