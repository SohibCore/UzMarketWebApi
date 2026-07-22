using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.CartDtos;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.Security;
using UzMarket.ServiceLayer.Services.CartServices.QueryObjects;

namespace UzMarket.ServiceLayer.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CartService(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<List<CartListDto>> GetListAsync(CartFilterDto filter)
        {
            var cart = await _context.Carts
                .Where(x => x.StatusId != (int)StatusIdConst.DELETED && x.UserId == _service.UserId)
                .Select(x => new CartListDto
                {
                    Id = x.Id,
                    StatusId = x.StatusId,
                    UserId = x.UserId
                }).SortFilter(filter)
                .ToListAsync();

            return cart ?? new List<CartListDto>();
        }

        public async Task<CartDto> GetAsync(long Id)
        {
            var cart = await _context.Carts
                .Include(x => x.Tables)
                .Include(x => x.User)
                .Where(x => x.UserId == _service.UserId)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (cart == null)
                throw new KeyNotFoundException($"Cart not found : {Id}");

            return new CartDto
            {
                Id = Id,
                StatusId = cart.StatusId,
                UserId = cart.UserId,
                Tables = cart.Tables.Select(x => new CartItemDto
                {
                    Id = x.Id,
                    CartId = x.CartId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };
        }

        public async Task<CartDto> CreateAsync(CreateCartDlDto dto, CancellationToken cancellationToken)
        {
            var tables = dto.Tables ?? new List<CreateCartItemDlDto>();

            var cart = new Cart
            {
                StatusId = (int)StatusIdConst.CREATED,
                UserId = _service.UserId,

                CreatedAt = DateTime.UtcNow,
                CreateUserId = _service.UserId,
                Tables = tables.Select(x => new CartItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            await _context.Carts.AddAsync(cart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CartDto
            {
                Id = cart.Id,
                StatusId = cart.StatusId,
                UserId = cart.UserId,
                Tables = cart.Tables.Select(x => new CartItemDto
                {
                    Id = x.Id,
                    CartId = x.CartId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };
        }

        public async Task<CartDto> UpdateAsync(UpdateCartDlDto dto, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Where(x => x.StatusId != (int)StatusIdConst.DELETED && x.UserId == _service.UserId)
                .Include(x => x.Tables)
                .FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);

            if (cart == null)
                throw new Exception("Cart not found");

            cart.StatusId = (int)StatusIdConst.MODIFIED;
            cart.ModifiedAt = DateTime.UtcNow;

            foreach (var itemDto in dto.Tables)
            {
                if (itemDto.Id.HasValue)
                {
                    var existingItem = cart.Tables.FirstOrDefault(x => x.Id == itemDto.Id.Value);
                    if (existingItem == null)
                        throw new Exception($"CartItem {itemDto.Id} not found");

                    existingItem.Quantity = itemDto.Quantity;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity
                    };
                    cart.Tables.Add(newItem);
                }
            }
                await _context.SaveChangesAsync(cancellationToken);

                return new CartDto
                {
                    Id = cart.Id,
                    StatusId = cart.StatusId,
                    Tables = cart.Tables.Select(x => new CartItemDto
                    {
                        Id = x.Id,
                        CartId = x.CartId,
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    }).ToList()
                };
            }

        public async Task<string> DeleteAsync(long Id, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.Id == Id && x.UserId == _service.UserId);

            if (cart == null || cart.StatusId == (int)StatusIdConst.DELETED)
                return $"Order with ID {Id} already deleted";

            cart.StatusId = (int)StatusIdConst.DELETED;
            await _context.SaveChangesAsync(cancellationToken);

            return $"Order with ID {Id} has been deleted successfully.";
        }
    }
}
