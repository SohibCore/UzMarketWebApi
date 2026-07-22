using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.Services.OrderServices.QueryObjects;
using UzMarket.ServiceLayer.Services.ProductServices;

namespace UzMarket.ServiceLayer.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IProductService _product;
        public OrderService(AppDbContext context, IProductService product)
        {
            _context = context;
            _product = product;
        }

        public async Task<List<OrderListDto>> GetListAsync(OrderFilterDto filter)
        {
            var order = await _context.Orders
                .Where(x => x.StatusId != (int)StatusIdConst.DELETED)
                .Select(x => new OrderListDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    OrderStatusId = x.OrderStatusId,
                    ShippingAddressId = x.ShippingAddressId
                }).SortFilter(filter)
            .ToListAsync();

            return order;
        }

        public async Task<OrderDto> GetAsync(long Id)
        {
            var order = await _context.Orders
                .Include(x => x.Tables)
                .Where(x => x.StatusId != (int)StatusIdConst.DELETED)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (order is null)
                throw new Exception($"Order not found : {Id}");

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatusId = order.OrderStatusId,
                ShippingAddressId = order.ShippingAddressId,
                Tables = order.Tables.Select(x => new OrderItemDto
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                }).ToList()
            };
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDlDto dto, CancellationToken cancellation)
        {
            var order = new Order
            {
                OrderDate = dto.OrderDate,
                OrderStatusId = (int)OrderStatus.PENDING,
                ShippingAddressId = dto.ShippingAddressId,

                CreatedAt = DateTime.UtcNow
            };

            decimal totalAmount = 0;

            foreach (var itemDto in dto.Tables)
            {
                // Mahsulotni bazadan olish
                var product = await _product.GetAsync(itemDto.ProductId)
                    ?? throw new Exception($"Product not found: {itemDto.ProductId}");

                var unitPrice = product.Price;
                var itemTotal = unitPrice * itemDto.Quantity;

                order.Tables.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = unitPrice
                });

                totalAmount += itemTotal;

                if (product.StockQuantity < itemDto.Quantity)
                    throw new Exception($"Yetarli miqdorda mahsulot yo'q: {product.Name}");

                product.StockQuantity -= itemDto.Quantity;
            }
            order.TotalAmount = totalAmount;

            await _context.Orders.AddAsync(order, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return new OrderDto
            {
                Id = order.Id,
                OrderDate = dto.OrderDate,
                OrderStatusId = (int)OrderStatus.PENDING,
                ShippingAddressId = dto.ShippingAddressId,
                TotalAmount = totalAmount,
            };
        }

        public async Task<OrderDto> UpdateAsync(UpdateOrderDlDto dto, CancellationToken cancellation)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellation);

            if (order == null)
                throw new Exception("Order not found");

            if (dto.ShippingAddressId != null && dto.ShippingAddressId != order.ShippingAddressId)
                order.ShippingAddressId = dto.ShippingAddressId.Value;

            order.Tables = dto.Tables.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = x.Price,
            }).ToList();

            await _context.SaveChangesAsync(cancellation);

            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderStatusId = (int)OrderStatus.PENDING,
                ShippingAddressId = order.ShippingAddressId,
                TotalAmount = order.TotalAmount,
                Tables = order.Tables.Select(x => new OrderItemDto
                {
                    Id = x.OrderId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price,
                }).ToList()
            };
        }

        public async Task<string> DeleteAsync(long Id, CancellationToken cancellation)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == Id, cancellation);

            if (order is null)
                return "Order not found";

            if (order.StatusId == (int)StatusIdConst.DELETED)
                return $"Order with ID {Id} already deleted";

            order.StatusId = (int)StatusIdConst.DELETED;
            await _context.SaveChangesAsync(cancellation);
            return $"Order with ID {Id} has been deleted successfully.";
        }
    }
}
