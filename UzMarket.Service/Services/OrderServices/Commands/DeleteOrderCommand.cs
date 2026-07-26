using MediatR;
using UzMarket.Core;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Commands
{
    public record DeleteOrderCommand(long Id) : IRequest<bool>;

    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public DeleteOrderHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellation)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == _service.UserId && x.StatusId != (int)StatusIdConst.DELETED, cancellation);

            if (order == null)
                throw new NotFoundException("Order not found");

            order.StatusId = (int)StatusIdConst.DELETED;
            order.ModifiedAt = DateTime.UtcNow;
            order.ModifiedUserId = _service.UserId;
            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}
