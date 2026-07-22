using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.AuthDtos;

namespace UzMarket.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("The UserName field is required.");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The Password field is required.");
        }

    }
}
