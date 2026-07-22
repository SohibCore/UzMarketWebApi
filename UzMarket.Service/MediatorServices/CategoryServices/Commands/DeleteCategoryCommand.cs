using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;

namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Commands
{
    public record DeleteCategoryCommand(long Id) : IRequest<string>;

    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, string>
    {
        private readonly AppDbContext _context;
        public DeleteCategoryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .Where(x => x.StatusId != (int)StatusIdConst.DELETED)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (category == null)
                throw new Exception($"{request.Id} not found");

            if (category.StatusId == (int)StatusIdConst.DELETED)
                throw new Exception($"Category with ID {request.Id} already deleted");

            category.StatusId = (int)StatusIdConst.DELETED;

            await _context.SaveChangesAsync(cancellationToken);
            return $"Category with ID {request.Id} has been deleted successfully."; ;
        }
    }
}
