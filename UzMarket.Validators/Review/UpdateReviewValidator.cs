using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.ReviewDtos;

namespace UzMarket.Validators.Review
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewDlDto>
    {
        public UpdateReviewValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.RatingId)
                .NotEmpty().WithMessage("The Rating field is required.")
                .InclusiveBetween(1, 5).WithMessage("The Rating field is required.")
                .When(x => x.RatingId != null);

            RuleFor(x => x.Comment)
                .MinimumLength(500).WithMessage("The Comment must be at least 5 character.")
                .MaximumLength(1000).WithMessage("The Comment must not exceed 200 characters.");
        }
    }
}
