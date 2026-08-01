using UzMarket.ServiceLayer.MediatorServices.OrderServices.Dtos;

namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Queries.QueryObjects
{
    public static class OrderSortFilter
    {
        public static IQueryable<OrderListDto> SortFilter(this IQueryable<OrderListDto> query, OrderFilterDto filter)
        {
            if (filter.TotalAmount.HasValue)
                query = query.Where(x => x.TotalAmount == filter.TotalAmount.Value);

            if (filter.OrderStatusId.HasValue)
                query = query.Where(x => x.OrderStatusId == filter.OrderStatusId.Value);

            if (filter.ShippingAddressId.HasValue)
                query = query.Where(x => x.ShippingAddressId == filter.ShippingAddressId.Value);

            return query;
        }
    }
}
