using MediatR;
using UzMarket.Core;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.FavoriteDtos;
using UzMarket.ServiceLayer.Security.AccountServices;
using UzMarket.ServiceLayer.Services.FavoriteServices.Dtos;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

namespace UzMarket.ServiceLayer.Services.FavoriteServices.Commands
{
    public record CreateFavoriteCommand(CreateFavoriteDlDto dto) : IRequest<FavoriteDto>;

    public class CreateFavoriteHandler : IRequestHandler<CreateFavoriteCommand, FavoriteDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CreateFavoriteHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<FavoriteDto> Handle(CreateFavoriteCommand request, CancellationToken cancellation)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == request.dto.ProductId && x.StatusId != (int)StatusIdConst.DELETED);

            if (product == null)
                throw new NotFoundException($"Product not found : {request.dto.ProductId}");

            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == _service.UserId && f.ProductId == request.dto.ProductId, cancellation);

            if (existingFavorite != null)
                throw new Exception($"Favorite already exists for product : {request.dto.ProductId}");

            var favorite = new Favorite
            {
                UserId = _service.UserId,
                ProductId = request.dto.ProductId,
                StatusId = StatusIdConst.CREATED
            };

            await _context.Favorites.AddAsync(favorite, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return new FavoriteDto
            {
                Id = favorite.Id,
                UserId = favorite.UserId,
                ProductId = favorite.ProductId
            };
        }
    }
}
