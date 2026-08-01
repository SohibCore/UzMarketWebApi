using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;
using UzMarket.ServiceLayer.Security.AccountServices;

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
            var category = await _context.Categories.AnyAsync(x => x.Id == request.dto.CategoryId && x.StatusId != (int)StatusIdConst.DELETED, cancellationToken);

            if (!category)
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

                //Selectda indexlab olindi va Asosiy rasmga to'g'irlandi
                Tables = request.dto.Items.Select((x, Index) => new ProductImage
                {
                    ImageUrl = x.ImageUrl,
                    MainPic = Index == 0,
                    SortOrder = x.SortOrder,
                }).ToList()
            };
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }
}
