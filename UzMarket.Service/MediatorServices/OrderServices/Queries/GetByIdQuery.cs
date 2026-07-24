using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.ServiceLayer.Security;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.OrderServices.Dtos;

namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Queries
{
    public record GetByIdQuery(long Id) : IRequest<OrderDto>;

    public class GetByIdHandler : IRequestHandler<GetByIdQuery, OrderDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetByIdHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<OrderDto> Handle(GetByIdQuery request, CancellationToken cancellation)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.StatusId != (int)StatusIdConst.DELETED && x.UserId == _service.UserId, cancellation);

            if (order == null)
                throw new Exception($"Order not found : {request.Id}");

            return new OrderDto
            {
                Id = request.Id,
                OrderDate = order.OrderDate,
                OrderStatusId = order.OrderStatusId,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(x => new OrderItemDto
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price,
                }).ToList()
            };
        }
    }
}
