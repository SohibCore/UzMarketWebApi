using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security.AccountServices;
using UzMarket.ServiceLayer.Services.FavoriteServices.Dtos;
using UzMarket.ServiceLayer.Services.FavoriteServices.Queries.QueryObjects;

namespace UzMarket.ServiceLayer.Services.FavoriteServices.Queries
{
    public record GetListQuery(FavoriteFilterDto filter) : IRequest<List<FavoriteListDto>>;

    public class GetListHandler : IRequestHandler<GetListQuery, List<FavoriteListDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetListHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<List<FavoriteListDto>> Handle(GetListQuery request, CancellationToken cancellation)
        {
            var favorites = await _context.Favorites
                .AsNoTracking()
                .Where(x => x.StatusId != Core.StatusIdConst.DELETED && x.UserId == _service.UserId)
                .Select(x => new FavoriteListDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Price = x.Product.Price,
                    StockQuantity = x.Product.StockQuantity,
                    ImageUrl = x.Product.Tables.Select(images => images.ImageUrl).FirstOrDefault(),
                }).SortFilter(request.filter)
                .ToListAsync(cancellation);

            return favorites;
        }
    }
}
