using FluentValidation;
using UzMarket.RepositoryLayer.Dtos.FavoriteDtos;

namespace UzMarket.Validators.Favorite
{
    public class CreateFavoriteValidator : AbstractValidator<CreateFavoriteDlDto>
    {
        public CreateFavoriteValidator()
        {
            RuleFor(x => x.ProductId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The Product field is required.")
                .GreaterThan(0).WithMessage("The Product field must be a valid product.");
        }
    }
}
