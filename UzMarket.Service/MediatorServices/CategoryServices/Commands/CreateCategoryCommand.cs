using MediatR;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.CategoryDtos;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.MediatorServices.CategoryServices.Dtos;
using UzMarket.ServiceLayer.Security;

namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Commands
{
    public record CreateCategoryCommand(CreateCategoryDlDto dto) : IRequest<CategoryDto>;

    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CreateCategoryHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
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

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync(cancellationToken);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
            };
        }
    }
}
