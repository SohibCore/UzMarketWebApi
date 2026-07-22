using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.UserDtos;

namespace UzMarket.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDlDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("UserName cannot be empty")
                .MinimumLength(6).WithMessage("The UserName must be at least 6 character")
                .MaximumLength(100).WithMessage("The UserName must not exceed 100 characters")
                .When(x => x.UserName != null);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(6).WithMessage("The Password must be at least 6 character")
                .MaximumLength(50).WithMessage("The Password must not exceed 50 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one upper character")
                .Matches("[a-z]").WithMessage("Password must contain at lower one upper character")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit")
                .When(x => x.Password != null);

            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("FullName cannot be empty")
                .MinimumLength(6).WithMessage("The FullName must be at least 6 character")
                .MaximumLength(500).WithMessage("The FullName must not exceed 500 characters")
                .When(x => x.FullName != null);

            RuleFor(x => x.ShortName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("ShortName cannot be empty")
                .MinimumLength(3).WithMessage("The ShortName must be at least 3 character")
                .MaximumLength(300).WithMessage("The ShortName must not exceed 300 characters")
                .When(x => x.ShortName != null);

            RuleFor(x => x.Pinfl)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Pinfl cannot be empty")
                .MaximumLength(14).WithMessage("The Pinfl must not exceed 14 characters")
                .When(x => x.Pinfl != null); ;

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("PhoneNumber cannot be empty")
                .Matches(@"^\+998(9[0-9]|3[3]|7[1257])\d{7}$")
                .WithMessage("The Phone number must be a valid Uzbek phone number")
                .When(x => x.PhoneNumber != null);

            RuleFor(x => x.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Address cannot be empty")
                .MinimumLength(3).WithMessage("The Address must be at least 3 character")
                .MaximumLength(300).WithMessage("The Address must not exceed 300 characters")
                .When(x => x.Address != null);

            RuleFor(x => x.DateOfBirth)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("DateOfBirth cannot be empty")
                .Matches(@"^(0[1-9]|[12]\d|3[01])\.(0[1-9]|1[0-2])\.\d{4}$")
                .WithMessage("The BirthDay must be in format dd.MM.yyyy.")
                .When(x => x.DateOfBirth != null);

            RuleFor(x => x.PassportSeries)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("PassportSeries cannot be empty")
                .Matches(@"^[A-Z]{2}\d{7}$")
                .WithMessage("The Passport series must be in format AA1234567.")
                .When(x => x.PassportSeries != null);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Email format is invalid")
                .When(x => x.Email != null);
        }
    }
}
