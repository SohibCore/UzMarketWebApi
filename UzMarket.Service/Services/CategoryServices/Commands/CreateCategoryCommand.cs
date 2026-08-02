using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.CategoryDtos;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Commands
{
    public record CreateCategoryCommand(CreateCategoryDlDto dto) : IRequest<long>;

    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, long>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CreateCategoryHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<long> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.dto.Name,
                Description = request.dto.Description,
                ParentCategoryId = request.dto.ParentCategoryId,

                StatusId = (int)StatusIdConst.CREATED,
                CreatedAt = DateTime.UtcNow,
                CreateUserId = _service.UserId
            };

            var categoryName = await _context.Categories.AnyAsync(x => x.Name == category.Name && x.StatusId != (int)StatusIdConst.DELETED, cancellationToken);
            if (categoryName)
                throw new Exception("Category already exists");

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}
