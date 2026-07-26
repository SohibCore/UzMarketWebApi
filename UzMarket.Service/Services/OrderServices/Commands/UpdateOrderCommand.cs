using MediatR;
using UzMarket.Core;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Commands
{
    public record UpdateOrderCommand(UpdateOrderDlDto dto) : IRequest<bool>;

    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public UpdateOrderHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellation)
        {
            var order = await _context.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == request.dto.Id && x.UserId == _service.UserId && x.StatusId != (int)StatusIdConst.DELETED, cancellation);

            if (order == null)
                throw new NotFoundException($"Order not found");

            var productId = request.dto.Tables.Select(x => x.ProductId).ToList();

            var products = await _context.Products.Where(x => productId.Contains(x.Id) && x.StatusId != (int)StatusIdConst.DELETED).ToListAsync(cancellation);

            if (request.dto.OrderDate != null)
                order.OrderDate = request.dto.OrderDate;

            order.StatusId = (int)StatusIdConst.MODIFIED;
            order.ModifiedAt = DateTime.UtcNow;
            order.ModifiedUserId = _service.UserId;

            _context.OrderItems.RemoveRange(order.Items);
            order.Items.Clear();

            foreach (var item in request.dto.Tables)
            {
                var product = products.FirstOrDefault(x => x.Id == item.ProductId);

                if (product == null)
                    throw new NotFoundException($"Product not found : {item.ProductId}");

                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price,
                });
            }

            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}
