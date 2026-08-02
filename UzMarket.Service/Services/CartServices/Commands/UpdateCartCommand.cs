using MediatR;
using UzMarket.Core;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.CartDtos;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.CartServices.Commands
{
    public record UpdateCartCommand(UpdateCartDlDto dto) : IRequest<bool>;

    public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public UpdateCartHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<bool> Handle(UpdateCartCommand request, CancellationToken cancellation)
        {
            var cart = await _context.Carts
                .Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.Id == request.dto.Id && x.UserId == _service.UserId && x.StatusId != (int)StatusIdConst.DELETED, cancellation);

            if (cart == null)
                throw new NotFoundException($"{nameof(Cart)} with id {request.dto.Id} not found.");

            cart.StatusId = (int)StatusIdConst.MODIFIED;
            cart.ModifiedUserId = _service.UserId;
            cart.ModifiedAt = DateTime.UtcNow;

            _context.CartItems.RemoveRange(cart.Items);

            cart.Items = request.dto.Items.Select(x => new CartItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
            }).ToList();

            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}
