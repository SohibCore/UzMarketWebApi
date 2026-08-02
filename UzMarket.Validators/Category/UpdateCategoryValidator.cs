using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.CategoryDtos;

namespace UzMarket.Validators.Category
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDlDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The Name field is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Name cannot be empty.")
                .MinimumLength(3).WithMessage("The Name must be at least 3 character.")
                .MaximumLength(100).WithMessage("The Name must not exceed 50 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(3).WithMessage("The Description must be at least 3 character.")
                .MaximumLength(500).WithMessage("The Description must not exceed 500 characters.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Description cannot be empty.")
                .When(x => x.Description != null);

            RuleFor(x => x.ParentCategoryId)
                .GreaterThan(0).WithMessage("The Main Category field must be a valid category.");
        }
    }
}
