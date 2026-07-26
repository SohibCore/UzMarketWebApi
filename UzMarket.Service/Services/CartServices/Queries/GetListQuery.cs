using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.CartServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.CartServices.Queries.QueryObjects;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.CartServices.Queries
{
    public record GetListQuery(CartFilterDto filter) : IRequest<List<CartListDto>>;

    public class GetListHandler : IRequestHandler<GetListQuery, List<CartListDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetListHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<List<CartListDto>> Handle(GetListQuery request, CancellationToken cancellation)
        {
            var carts = await _context.Carts
                .AsNoTracking()
                .Where(x => x.UserId == _service.UserId && x.StatusId != (int)StatusIdConst.DELETED)
                .Select(x => new CartListDto
                {
                    Id = x.Id,
                    StatusId = x.StatusId,
                    Items = x.Items.Select(item => new CartItemDto
                    {
                        Id = item.Id,
                        Quantity = item.Quantity,
                        ProductId = item.ProductId,
                    }).ToList()
                }).SortFilter(request.filter)
                .ToListAsync(cancellation);

            return carts;
        }
    }
}
