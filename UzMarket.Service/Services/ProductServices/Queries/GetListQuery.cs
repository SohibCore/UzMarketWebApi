using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security.AccountServices;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Queries.ObjectQuery;

namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Queries
{
    public record GetListQuery(ProductFilterDto filter) : IRequest<List<ProductListDto>>;

    public class GetListHandler : IRequestHandler<GetListQuery, List<ProductListDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetListHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<List<ProductListDto>> Handle(GetListQuery request, CancellationToken cancellation)
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(x => x.StatusId != (int)StatusIdConst.DELETED && x.SupplierId == _service.UserId)
                .Select(x => new ProductListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity,
                    CategoryId = x.CategoryId,
                    SupplierId = x.SupplierId,
                    Tables = x.Tables.Select(x => new ProductImageDto
                    {
                        Id = x.Id,
                        ImageUrl = x.ImageUrl,
                        MainPic = x.MainPic,
                        SortOrder = x.SortOrder,
                    }).ToList()
                }).SortFilter(request.filter)
                .ToListAsync(cancellation);

            return products;
        }
    }
}
