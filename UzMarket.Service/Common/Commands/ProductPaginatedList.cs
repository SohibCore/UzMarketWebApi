using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.Core.Common;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos;

namespace UzMarket.ServiceLayer.Common.Commands
{
    public record ProductPaginatedListQuery(int pageNumber, int pageSize, string? search = null) : IRequest<PaginatedList<ProductDto>>;

    public class ProductPaginatedListHandler : IRequestHandler<ProductPaginatedListQuery, PaginatedList<ProductDto>>
    {
        private readonly AppDbContext _context;
        public ProductPaginatedListHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PaginatedList<ProductDto>> Handle(ProductPaginatedListQuery request, CancellationToken cancellation)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.search))
                query = query.Where(p => p.Name.Contains(request.search));

            query = query.OrderBy(p => p.Id);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity,
                    CategoryId = x.CategoryId,
                    Tables = x.Tables.Select(item => new ProductImageDto
                    {
                        Id = item.Id,
                        ImageUrl = item.ImageUrl,
                        MainPic = item.MainPic,
                        SortOrder = item.SortOrder,
                    }).ToList()
                })
                .ToListAsync();

            return new PaginatedList<ProductDto>(items, totalCount, request.pageNumber, request.pageSize);
        }
    }
}
