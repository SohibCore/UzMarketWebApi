using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.ServiceLayer.Security;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;

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
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.dto.Id && x.SupplierId == _service.UserId, cancellationToken);

            if (product == null)
                throw new Exception($"Product not found : {request.dto.Id}");

            product.Name = request.dto.Name;
            product.Description = request.dto.Description;
            product.Price = request.dto.Price;
            product.StockQuantity = request.dto.StockQuantity;
            product.CategoryId = request.dto.CategoryId;

            product.ModifiedUserId = _service.UserId;
            product.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
