using UzMarket.ServiceLayer.MediatorServices.AddressServices.Dtos;

namespace UzMarket.ServiceLayer.MediatorServices.AddressServices.Queries.ObjectQuery
{
    public static class AddressSortFilter
    {
        public static IQueryable<AddressListDto> SortFilter(this IQueryable<AddressListDto> query, AddressFilterDto filter)
        {
            if (filter.Id.HasValue)
                query = query.Where(x => x.Id == filter.Id.Value);

            if (!string.IsNullOrWhiteSpace(filter.Region))
                query = query.Where(x => x.Region.ToLower() == filter.Region.ToLower());

            if (!string.IsNullOrWhiteSpace(filter.City))
                query = query.Where(x => x.City.ToLower() == filter.City.ToLower());

            if (!string.IsNullOrWhiteSpace(filter.Street))
                query = query.Where(x => x.Street.ToLower() == filter.Street.ToLower());

            if (!string.IsNullOrWhiteSpace(filter.PostalCode))
                query = query.Where(x => x.PostalCode.ToLower() == filter.PostalCode.ToLower());

            return query;
        }
    }
}
