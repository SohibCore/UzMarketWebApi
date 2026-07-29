using MediatR;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security.AccountServices;
using UzMarket.ServiceLayer.Services.FavoriteServices.Dtos;

namespace UzMarket.ServiceLayer.Services.FavoriteServices.Queries
{
    public record GetByIdQuery(int Id) : IRequest<FavoriteDto>;

    public class GetByIdHandler : IRequestHandler<GetByIdQuery, FavoriteDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetByIdHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<FavoriteDto> Handle(GetByIdQuery request, CancellationToken cancellation)
        {
            var favorite = await _context.Favorites
                .AsNoTracking()
                .Where(x => x.Id == request.Id && x.StatusId != Core.StatusIdConst.DELETED && x.UserId == _service.UserId)
                .Select(x => new FavoriteDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Price = x.Product.Price,
                    StockQuantity = x.Product.StockQuantity,
                    ImageUrl = x.Product.Tables.Select(images => images.ImageUrl).FirstOrDefault(),
                }).FirstOrDefaultAsync(cancellation);

            if (favorite == null)
                throw new NotFoundException($"Favorite not found : {request.Id}");

            return favorite;
        }
    }
}
