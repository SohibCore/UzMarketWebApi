using UzMarket.ServiceLayer.Services.FavoriteServices.Dtos;

namespace UzMarket.ServiceLayer.Services.FavoriteServices.Queries.QueryObjects
{
    public static class FavoriteSortFilter
    {
        public static IQueryable<FavoriteListDto> SortFilter(this IQueryable<FavoriteListDto> query, FavoriteFilterDto filter)
        {
            if (filter.Id.HasValue)
                query = query.Where(x => x.Id == filter.Id.Value);

            if (filter.ProductId.HasValue)
                query = query.Where(x => x.ProductId == filter.ProductId.Value);

            return query;
        }
    }
}
