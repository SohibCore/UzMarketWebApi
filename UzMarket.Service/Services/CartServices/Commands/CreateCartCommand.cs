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
            var productId = request.dto.Items
                .Select(x => x.ProductId)
                .Distinct();

            var products = await _context.Products
                .Where(x => productId.Contains(x.Id) && x.StatusId != (int)StatusIdConst.DELETED)
                .ToListAsync(cancellation);

            if (products.Count != productId.Count())
                throw new NotFoundException("One or more products not found.");

            var item = products.FirstOrDefault();
            var itemProductId = request.dto.Items.First().ProductId;
            var product = await _context.Products.FindAsync(itemProductId);

            if (product == null)
                throw new NotFoundException($"Product {itemProductId} does not exist");

            if (product.StockQuantity < item.StockQuantity)
                throw new Exception("Insufficient stock.");

            var cart = new Cart
            {
                UserId = _service.UserId,
                StatusId = (int)StatusIdConst.CREATED,
                CreatedAt = DateTime.UtcNow,
                CreateUserId = _service.UserId,
                Items = request.dto.Items.Select(x => new CartItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                }).ToList()
            };

            await _context.Carts.AddAsync(cart, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return cart.Id;
        }
    }
}
