namespace UzMarket.ServiceLayer.Services.UserServices.QueryObjects
{
    public static class UserSortFilter
    {
        public static IQueryable<UserListDto> SortFilter(this IQueryable<UserListDto> query, UserFilterDto filter)
        {
            if (filter.Id.HasValue)
                query = query.Where(x => x.Id == filter.Id);

            if (!string.IsNullOrWhiteSpace(filter.UserName))
                query = query.Where(x => x.UserName == filter.UserName);

            if (!string.IsNullOrWhiteSpace(filter.FullName))
                query = query.Where(x => x.FullName == filter.FullName);

            if (!string.IsNullOrWhiteSpace(filter.ShortName))
                query = query.Where(x => x.ShortName == filter.ShortName);

            return query;
        }
    }
}
