namespace UzMarket.ServiceLayer.Services.OrderServices.QueryObjects
{
    public static class OrderSortFilter
    {
        public static IQueryable<OrderListDto> SortFilter(this IQueryable<OrderListDto> query, OrderFilterDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.OrderDate))
                query = query.Where(x => x.OrderDate == filter.OrderDate);

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
