using MediatR;
using UzMarket.Core;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.CartServices.Dtos;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.CartServices.Queries
{
    public record GetByIdQuery(long Id) : IRequest<CartDto>;

    public class CartByIdHandler : IRequestHandler<GetByIdQuery, CartDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CartByIdHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<CartDto> Handle(GetByIdQuery request, CancellationToken cancellation)
        {
            var cart = await _context.Carts
                .AsNoTracking()
                .Where(x => x.Id == request.Id && x.UserId == _service.UserId && x.StatusId != (int)StatusIdConst.DELETED)
                .Select(x => new CartDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    StatusId = x.StatusId,
                    Items = x.Items.Select(item => new CartItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    }).ToList()
                }).SingleOrDefaultAsync(cancellation);

            if (cart == null)
                throw new NotFoundException($"Cart not found : {request.Id}");
            return cart;
        }
    }
}
