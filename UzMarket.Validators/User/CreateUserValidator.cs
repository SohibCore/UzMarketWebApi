using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.UserDtos;

namespace UzMarket.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserDlDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The UserName field is required")
                .MinimumLength(6).WithMessage("The UserName must be at least 6 character")
                .MaximumLength(100).WithMessage("The UserName must not exceed 100 characters");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The Password field is required.")
                .MinimumLength(6).WithMessage("The Password must be at least 6 character")
                .MaximumLength(50).WithMessage("The Password must not exceed 50 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one upper character")
                .Matches("[a-z]").WithMessage("Password must contain at lower one upper character")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit");

            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The FullName field is required")
                .MinimumLength(6).WithMessage("The FullName must be at least 6 character")
                .MaximumLength(500).WithMessage("The FullName must not exceed 500 characters");

            RuleFor(x => x.ShortName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The ShortName field is required")
                .MinimumLength(3).WithMessage("The ShortName must be at least 3 character")
                .MaximumLength(300).WithMessage("The ShortName must not exceed 300 characters");

            RuleFor(X => X.Pinfl)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The Pinfl field is required")
                .Length(14).WithMessage("The Pinfl must be exactly 14 characters");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The PhoneNumber field is required")
                .Matches(@"^\+998(9[0-9]|3[3]|7[1257])\d{7}$")
                .WithMessage("The Phone number must be a valid Uzbek phone number");

            RuleFor(x => x.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The Address field is required")
                .MinimumLength(3).WithMessage("The Address must be at least 3 character")
                .MaximumLength(300).WithMessage("The Address must not exceed 300 characters");

            RuleFor(x => x.DateOfBirth)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The BirthDay field is required")
                .Matches(@"^(0[1-9]|[12]\d|3[01])\.(0[1-9]|1[0-2])\.\d{4}$")
                .WithMessage("The BirthDay must be in format dd.MM.yyyy.");

            RuleFor(x => x.PassportSeries)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The PassportSeries field is required")
                .Matches(@"^[A-Z]{2}\d{7}$")
                .WithMessage("The Passport series must be in format AA1234567.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The Email field is required")
                .EmailAddress().WithMessage("Email format is invalid");
        }
    }
}
