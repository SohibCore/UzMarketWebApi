using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.CategoryDtos;

namespace UzMarket.Validators.Category
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDlDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The Name field is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Name cannot be empty.")
                .MinimumLength(3).WithMessage("The Name must be at least 3 character.")
                .MaximumLength(100).WithMessage("The Name must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(3).WithMessage("The Description must be at least 3 character.")
                .MaximumLength(500).WithMessage("The Description must not exceed 500 characters.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Description cannot be empty.");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThan(0).WithMessage("The Main Category field must be a valid category.");
        }
    }
}
