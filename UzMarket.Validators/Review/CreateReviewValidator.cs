using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.ReviewDtos;

namespace UzMarket.Validators.Review
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewDlDto>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("The Product field is required.")
                .GreaterThan(0).WithMessage("The Product field must be a valid product.");

            RuleFor(x => x.RatingId)
                .NotEmpty().WithMessage("The Rating field is required.")
                .InclusiveBetween(1, 5).WithMessage("The Rating field is required.");

            RuleFor(x => x.Comment)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("The Comment field cannot be empty.")
                .MinimumLength(500).WithMessage("The Comment must be at least 5 character.")
                .MaximumLength(1000).WithMessage("The Comment must not exceed 200 characters.");
        }
    }
}
