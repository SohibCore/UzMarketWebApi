using UzMarket.ServiceLayer.MediatorServices.CategoryServices.Dtos;

namespace UzMarket.ServiceLayer.MediatorServices.CategoryServices.Queries.ObjectQuery
{
    public static class CategorySoltFilter
    {
        public static IQueryable<CategoryListDto> SortFilter(this IQueryable<CategoryListDto> query, CategoryFilterDto filter)
        {
            if (filter.Id.HasValue)
                query = query.Where(x => x.Id == filter.Id.Value);

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(x => x.Name.ToLower() == filter.Name.ToLower());

            return query;
        }
    }
}
