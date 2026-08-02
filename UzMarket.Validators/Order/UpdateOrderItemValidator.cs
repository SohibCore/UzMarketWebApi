using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;

namespace UzMarket.Validators
{
    public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemDlDto>
    {
        public UpdateOrderItemValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid Order Id is required to identify which item to update.");

            RuleFor(x => x.Quantity)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The Quantity field must be a valid quantity.");

            RuleFor(x => x.ProductId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("The Product field must be a valid address.");
        }
    }
}
