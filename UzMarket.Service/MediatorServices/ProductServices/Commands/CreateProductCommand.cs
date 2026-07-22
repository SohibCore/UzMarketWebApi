using MediatR;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.Security;

namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Commands
{
    public record CreateProductCommand(CreateProductDlDto dto) : IRequest<bool>;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CreateProductCommandHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.dto.Name,
                Description = request.dto.Description,
                Price = request.dto.Price,
                CategoryId = request.dto.CategoryId,
                StockQuantity = request.dto.StockQuantity,
                SupplierId = _service.UserId,

                CreatedAt = DateTime.UtcNow,
                CreateUserId = _service.UserId,

                Tables = request.dto.Tables.Select(x => new ProductImage
                {
                    ImageUrl = x.ImageUrl,
                    MainPic = x.MainPic,
                    SortOrder = x.SortOrder,
                    ProductId = x.ProductId

                }).ToList()
            };
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
