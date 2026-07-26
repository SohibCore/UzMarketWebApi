using MediatR;
using UzMarket.Core;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Queries
{
    public record GetByIdQuery(long Id) : IRequest<ProductDto>;

    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ProductDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetByIdQueryHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<ProductDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
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
                    Tables = x.Tables.Select(x => new ProductImageDto
                    {
                        Id = x.Id,
                        ImageUrl = x.ImageUrl,
                        MainPic = x.MainPic,
                        SortOrder = x.SortOrder,
                    }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);

            if (product == null)
                throw new NotFoundException($"Product not found : {request.Id}");

            return product;
        }
    }
}
