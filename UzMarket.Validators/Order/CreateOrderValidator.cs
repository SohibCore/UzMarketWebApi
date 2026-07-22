using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;

namespace UzMarket.Validators   
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDlDto>
    {
        public CreateOrderValidator(IValidator<CreateOrderItemDlDto> validator)
        {
            RuleFor(x => x.OrderDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The OrderDate field is required.")
                .Matches(@"^(0[1-9]|[12]\d|3[01])\.(0[1-9]|1[0-2])\.\d{4}$")
                .WithMessage("The OrderDate must be in format dd.MM.yyyy.");

            RuleFor(x => x.ShippingAddressId)
                .GreaterThan(0).WithMessage("The ShippingAddressId field must be a valid address.");

            RuleFor(x => x.Tables)
                .NotEmpty().WithMessage("The order must contain at least one item.");

            RuleForEach(x => x.Tables).SetValidator(validator);
        }
    }
}
