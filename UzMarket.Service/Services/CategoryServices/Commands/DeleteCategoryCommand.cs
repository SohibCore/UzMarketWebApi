using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Commands
{
    public record DeleteCategoryCommand(long Id) : IRequest<bool>;

    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public DeleteCategoryHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.StatusId != (int)StatusIdConst.DELETED);

            if (category == null)
                throw new Exception($"{request.Id} not found");

            if (category.StatusId == (int)StatusIdConst.DELETED)
                throw new Exception($"Category with ID {request.Id} already deleted");

            category.StatusId = (int)StatusIdConst.DELETED;
            category.ModifiedAt = DateTime.UtcNow;
            category.ModifiedUserId = _service.UserId;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
