using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;

namespace UzMarket.Validators
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDlDto>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("The Product field is required.")
                .GreaterThan(0).WithMessage("The ProductId field must be a valid product.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("The Quantity field is required.")
                .GreaterThan(0).WithMessage("The Quantity field must be a valid quantity.");
        }
    }
}
