using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.CategoryDtos;
using UzMarket.ServiceLayer.Security;

namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Commands
{
    public record UpdateCategoryCommand(UpdateCategoryDlDto dto) : IRequest<long>;

    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, long>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public UpdateCategoryHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<long> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.dto.Id);
            var userId = _service.UserId;

            if (userId == 0)
                throw new Exception($"{userId} not found for User");

            if (category == null)
                throw new Exception($"{request.dto.Id} - not found");

            if (request.dto.ParentCategoryId != null)
            {
                if (request.dto.ParentCategoryId == request.dto.Id)
                    throw new Exception($"Category cannot be its own parent");
            }

            if (category.Name != request.dto.Name)
                category.Name = request.dto.Name;

            if (request.dto.Description != null && category.Description != request.dto.Description)
                category.Description = request.dto.Description;

            if (request.dto.ParentCategoryId.HasValue)
                category.ParentCategoryId = request.dto.ParentCategoryId.Value;

            category.ModifiedAt = DateTime.UtcNow;
            category.ModifiedUserId = userId;

            await _context.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}
