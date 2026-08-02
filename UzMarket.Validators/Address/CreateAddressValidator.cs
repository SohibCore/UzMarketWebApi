using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.AddressDtos;

namespace UzMarket.Validators.Address
{
    public class CreateAddressValidator : AbstractValidator<CreateAddressDlDto>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.Region)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Region is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Region cannot be empty.")
                .MinimumLength(3).WithMessage("The Region must be at least 3 character.")
                .MaximumLength(100).WithMessage("The Region must not exceed 100 characters.");

            RuleFor(x => x.City)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("City is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The City cannot be empty.")
                .MinimumLength(3).WithMessage("The City must be at least 3 character.")
                .MaximumLength(100).WithMessage("The City must not exceed 100 characters.");

            RuleFor(x => x.Street)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Street is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Street cannot be empty.")
                .MinimumLength(3).WithMessage("The Street must be at least 3 character.")
                .MaximumLength(100).WithMessage("The Street must not exceed 100 characters.");

            RuleFor(x => x.PostalCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("PostalCode is required.")
                .Matches(@"^\d{6}$").WithMessage("The PostalCode must contain exactly 6 digits.");
        }
    }
}
