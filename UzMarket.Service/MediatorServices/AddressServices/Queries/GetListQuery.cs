using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Queries.ObjectQuery;

namespace UzMarket.ServiceLayer.MediatorServices.AddressServices.Queries
{
    public record GetListQuery(AddressFilterDto dto) : IRequest<List<AddressListDto>>;

    public class GetListQueryHandler : IRequestHandler<GetListQuery, List<AddressListDto>>
    {
        private readonly AppDbContext _context;
        public GetListQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AddressListDto>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var address = await _context.Addresses
                .Select(x => new AddressListDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Region = x.Region,
                    City = x.City,
                    PostalCode = x.PostalCode,
                    IsDefault = x.IsDefault,
                    Street = x.Street,
                }).SortFilter(request.dto)
                 .ToListAsync(cancellationToken);

            return address;
        }
    }
}
