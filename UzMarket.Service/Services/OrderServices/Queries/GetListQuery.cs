using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.OrderServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.OrderServices.Queries.QueryObjects;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Queries
{
    public record GetListQuery(OrderFilterDto filter) : IRequest<List<OrderListDto>>;

    public class GetListHandler : IRequestHandler<GetListQuery, List<OrderListDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetListHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<List<OrderListDto>> Handle(GetListQuery request, CancellationToken cancellation)
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Where(x => x.StatusId != (int)StatusIdConst.DELETED && x.UserId == _service.UserId)
                .Select(x => new OrderListDto
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate,
                    OrderStatusId = x.StatusId,
                    TotalAmount = x.TotalAmount,
                    Items = x.Items.Select(x => new OrderItemDto
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        Price = x.Price,
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                    }).ToList()
                }).SortFilter(request.filter)
                .ToListAsync(cancellation);

            return orders;
        }
    }
}
