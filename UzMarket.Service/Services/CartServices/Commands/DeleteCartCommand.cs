using MediatR;
using UzMarket.Core;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.CartServices.Commands
{
    public record DeleteCartCommand(long Id) : IRequest<bool>;

    public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public DeleteCartHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<bool> Handle(DeleteCartCommand request, CancellationToken cancellation)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == _service.UserId && x.StatusId != (int)StatusIdConst.DELETED, cancellation);

            if (cart == null)
                throw new NotFoundException($"Cart not found : {request.Id}");

            cart.StatusId = (int)StatusIdConst.DELETED;
            cart.ModifiedAt = DateTime.UtcNow;
            cart.ModifiedUserId = _service.UserId;
            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}
