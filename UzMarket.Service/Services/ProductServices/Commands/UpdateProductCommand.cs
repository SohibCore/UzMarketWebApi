using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Commands
{
    public record UpdateProductCommand(UpdateProductDlDto dto) : IRequest<bool>;

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public UpdateProductHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(x => x.Tables)
                .FirstOrDefaultAsync(x => x.Id == request.dto.Id && x.SupplierId == _service.UserId, cancellationToken);

            if (product == null)
                throw new Exception($"Product not found : {request.dto.Id}");

            if (!string.IsNullOrWhiteSpace(request.dto.Name))
                product.Name = request.dto.Name;
            if (!string.IsNullOrWhiteSpace(request.dto.Description))
                product.Description = request.dto.Description;
            if (request.dto.Price.HasValue)
                product.Price = request.dto.Price.Value;
            if (request.dto.StockQuantity.HasValue)
                product.StockQuantity = request.dto.StockQuantity.Value;
            if (request.dto.CategoryId.HasValue)
                product.CategoryId = request.dto.CategoryId.Value;

            product.ModifiedUserId = _service.UserId;
            product.ModifiedAt = DateTime.UtcNow;

            if (request.dto.Items is not null)
            {
                _context.ProductImages.RemoveRange(product.Tables);
                product.Tables = request.dto.Items.Select((x, index) => new ProductImage
                {
                    ImageUrl = x.ImageUrl ?? string.Empty,
                    MainPic = index == 0,
                    SortOrder = x.SortOrder ?? index + 1,
                }).ToList();
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
