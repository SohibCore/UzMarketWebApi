using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Dtos;
using UzMarket.ServiceLayer.Security;

namespace UzMarket.ServiceLayer.MediatorServices.AddressServices.Queries
{
    public record GetByIdQuery(int Id) : IRequest<AddressDto>;
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, AddressDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetByIdQueryHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<AddressDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == _service.UserId, cancellationToken);

            if (address == null)
                throw new Exception($"{request.Id} not found");

            return new AddressDto
            {
                Id = request.Id,
                UserId = _service.UserId,
                Region = address.Region,
                City = address.City,
                IsDefault = address.IsDefault,
                PostalCode = address.PostalCode,
                Street = address.Street,
            };
        }
    }
}
