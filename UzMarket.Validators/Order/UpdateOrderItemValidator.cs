using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;

namespace UzMarket.Validators
{
    public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemDlDto>
    {
        public UpdateOrderItemValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("The Quantity field must be a valid quantity.")
                .When(x => x.Quantity != null);

            RuleFor(x => x.ProductId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The ProductId field must be a valid address.")
                .When(x => x.ProductId != null);

            RuleFor(x => x.Price)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The Price field must be a valid quantity.")
                .When(x => x.Price != null);
        }
    }
}
