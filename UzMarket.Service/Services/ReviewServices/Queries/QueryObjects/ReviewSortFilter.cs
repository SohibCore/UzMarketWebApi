using UzMarket.ServiceLayer.Services.ReviewServices.Dtos;

namespace UzMarket.ServiceLayer.Services.ReviewServices.Queries.QueryObjects
{
    public static class ReviewSortFilter
    {
        public static IQueryable<ReviewListDto> SortFilter(this IQueryable<ReviewListDto> query, ReviewFilterDto filter)
        {
            if (filter.StatusId.HasValue)
                query = query.Where(r => r.StatusId == filter.StatusId.Value);
            if (filter.Id.HasValue)
                query = query.Where(r => r.Id == filter.Id.Value);

            return query;
        }
    }
}
