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
    public record CreateOrderCommand(CreateOrderDlDto dto) : IRequest<bool>;

    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CreateOrderHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellation)
        {
            var order = new Order
            {
                UserId = _service.UserId,
                OrderDate = request.dto.OrderDate,
                OrderStatusId = (int)OrderStatus.PENDING,
                ShippingAddressId = request.dto.ShippingAddressId,

                CreatedAt = DateTime.UtcNow,
                CreateUserId = _service.UserId,
            };

            var address = await _context.Addresses.SingleOrDefaultAsync(x => x.Id == order.ShippingAddressId && x.StatusId != (int)StatusIdConst.DELETED && x.UserId == _service.UserId, cancellation);

            if (address == null)
                throw new NotFoundException("Address not found.");

            decimal totalAmount = 0;

            foreach (var itemDto in request.dto.Items)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == itemDto.ProductId && x.StatusId != (int)StatusIdConst.DELETED, cancellation);

                if (product == null)
                    throw new NotFoundException($"Product #{itemDto.ProductId} not found");

                var unitPrice = product.Price;
                var itemTotal = unitPrice * itemDto.Quantity;

                totalAmount += itemTotal;

                if (product.StockQuantity < itemDto.Quantity)
                    throw new InvalidOperationException($"Yetarli miqdorda mahsulot yo'q: {product.Name}");

                product.StockQuantity -= itemDto.Quantity;

                order.Items.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = unitPrice
                });
            }
            order.TotalAmount = totalAmount;

            await _context.Orders.AddAsync(order, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return true;
        }
    }
}
