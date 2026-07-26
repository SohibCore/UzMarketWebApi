using UzMarket.ServiceLayer.MediatorServices.CartServices.Dtos;

namespace UzMarket.ServiceLayer.MediatorServices.CartServices.Queries.QueryObjects
{
    public static class CartSortFilter
    {
        public static IQueryable<CartListDto> SortFilter(this IQueryable<CartListDto> query, CartFilterDto filter)
        {
            if (filter.Id.HasValue)
                query = query.Where(x => x.Id == filter.Id.Value);

            return query;
        }
    }
}
