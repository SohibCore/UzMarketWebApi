using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.ServiceLayer.Security;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;

namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Commands
{
    public record CreateProductCommand(CreateProductDlDto dto) : IRequest<long>;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, long>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CreateProductCommandHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.dto.CategoryId, cancellationToken);

            if (category == null)
                throw new Exception($"Category not found : {request.dto.CategoryId}");

            var product = new Product
            {
                Name = request.dto.Name,
                Description = request.dto.Description,
                Price = request.dto.Price,
                CategoryId = request.dto.CategoryId,
                StockQuantity = request.dto.StockQuantity,
                SupplierId = _service.UserId,
                StatusId = (int)StatusIdConst.CREATED,

                CreatedAt = DateTime.UtcNow,
                CreateUserId = _service.UserId,

                Tables = request.dto.Tables.Select(x => new ProductImage
                {
                    ImageUrl = x.ImageUrl,
                    MainPic = x.MainPic,
                    SortOrder = x.SortOrder,
                }).ToList()
            };

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }
}
