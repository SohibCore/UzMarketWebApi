using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.ServiceLayer.Security;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos;

namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Queries
{
    public record GetByIdQuery(long Id) : IRequest<List<ProductDto>>;

    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, List<ProductDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetByIdQueryHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<List<ProductDto>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Where(x => x.Id == request.Id && x.SupplierId == _service.UserId && x.StatusId != (int)StatusIdConst.DELETED)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    StockQuantity = x.StockQuantity,
                    SupplierId = x.SupplierId,
                    Tables = product.Tables(x => new ProductImageDto
                    {

                    })
                }).ToListAsync(cancellationToken);

            if (product == null)
                throw new Exception($"Product not found : {request.Id}");

            return product;
        }
    }
}
