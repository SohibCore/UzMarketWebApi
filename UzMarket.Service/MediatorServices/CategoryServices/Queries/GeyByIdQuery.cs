using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.CategoryServices.Dtos;
using UzMarket.ServiceLayer.Security;

namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Queries
{
    public record GetByIdQuery(long Id) : IRequest<CategoryDto>;

    public class GetByIdHandler : IRequestHandler<GetByIdQuery, CategoryDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetByIdHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<CategoryDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .Where(x => x.StatusId != (int)StatusIdConst.DELETED)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (category == null)
                throw new Exception($"Category not found : {request.Id}");

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
