using UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos;

namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Queries.QueryObejcts
{
    public static class ProductSortFilter
    {
        public static IQueryable<ProductListDto> SortFilter(this IQueryable<ProductListDto> query, ProductFilterDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(x => x.Name == filter.Name);

            if (!string.IsNullOrWhiteSpace(filter.Description))
                query = query.Where(x => x.Description == filter.Description);

            if (filter.Price.HasValue)
                query = query.Where(x => x.Price == filter.Price.Value);

            return query;
        }
    }
}
