using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.ServiceLayer.Security;
using UzMarket.RepositoryLayer.DataBase;

namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Commands
{
    public record DeleteProductCommand(long Id) : IRequest<bool>;
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public DeleteProductHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id && x.SupplierId == _service.UserId && x.StatusId != (int)StatusIdConst.DELETED, cancellationToken);

            if (product == null)
                throw new Exception($"Product not found : {request.Id}");

            product.StatusId = (int)StatusIdConst.DELETED;
            product.ModifiedUserId = _service.UserId;
            product.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
