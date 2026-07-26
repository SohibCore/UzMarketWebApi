using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Queries.ObjectQuery;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.AddressServices.Queries
{
    public record GetListQuery(AddressFilterDto dto) : IRequest<List<AddressListDto>>;

    public class GetListQueryHandler : IRequestHandler<GetListQuery, List<AddressListDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetListQueryHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<List<AddressListDto>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var address = await _context.Addresses
                .AsNoTracking()
                .Where(x => x.StatusId != (int)StatusIdConst.DELETED && x.UserId == _service.UserId)
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
