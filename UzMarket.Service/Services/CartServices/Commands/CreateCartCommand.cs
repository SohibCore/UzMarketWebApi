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
    public record CreateCartCommand(CreateCartDlDto dto) : IRequest<long>;

    public class CreateCartHandler : IRequestHandler<CreateCartCommand, long>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CreateCartHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<long> Handle(CreateCartCommand request, CancellationToken cancellation)
        {
            var cart = new Cart
            {
                UserId = _service.UserId,
                StatusId = (int)StatusIdConst.CREATED,
                CreatedAt = DateTime.UtcNow,
                CreateUserId = _service.UserId,
                Items = new List<CartItem>()
            };

            if (request.dto?.Items != null && request.dto.Items.Any())
            {
                var productIds = request.dto.Items
                    .Select(x => x.ProductId)
                    .Distinct()
                    .ToList();

                var products = await _context.Products
                    .Where(x => productIds.Contains(x.Id) && x.StatusId != (int)StatusIdConst.DELETED)
                    .ToDictionaryAsync(x => x.Id, cancellation);

                if (products.Count != productIds.Count)
                    throw new NotFoundException("One or more products not found.");

                foreach (var itemDto in request.dto.Items)
                {
                    if (products.TryGetValue(itemDto.ProductId, out var prod))
                    {
                        if (prod.StockQuantity < itemDto.Quantity)
                            throw new Exception($"Insufficient stock for product {prod.Name}.");

                        cart.Items.Add(new CartItem
                        {
                            ProductId = itemDto.ProductId,
                            Quantity = itemDto.Quantity,
                        });
                    }
                }
            }

            await _context.Carts.AddAsync(cart, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return cart.Id;
        }
    }
}
