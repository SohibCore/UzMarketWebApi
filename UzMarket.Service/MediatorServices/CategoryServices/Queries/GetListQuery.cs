using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.CategoryServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.CategoryServices.Queries.ObjectQuery;

namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Queries
{
    public record GetListQuery(CategoryFilterDto dto) : IRequest<List<CategoryListDto>>;

    public class GetListHandler : IRequestHandler<GetListQuery, List<CategoryListDto>>
    {
        private readonly AppDbContext _context;
        public GetListHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryListDto>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.Select(x => new CategoryListDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ParentCategoryId = x.ParentCategoryId,
            }).SortFilter(request.dto)
            .ToListAsync();

            if (category == null || category.Count == 0)
                throw new Exception("Category not found");

            return category;
        }
    }
}
