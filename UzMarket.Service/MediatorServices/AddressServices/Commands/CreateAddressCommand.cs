using MediatR;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.AddressDtos;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Dtos;
using UzMarket.ServiceLayer.Security;

namespace UzMarket.ServiceLayer.MediatorServices.AddressServices.Commands
{
    public record CreateAddressCommand(CreateAddressDlDto dto) : IRequest<AddressDto>;

    public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, AddressDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CreateAddressHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<AddressDto> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _service.UserId;

            if (userId == 0)
                throw new UnauthorizedAccessException("UserId topilmadi");

            var address = new Address
            {
                UserId = userId,
                Region = request.dto.Region,
                City = request.dto.City,
                Street = request.dto.Street,
                PostalCode = request.dto.PostalCode,
                IsDefault = request.dto.IsDefault,

                CreatedAt = DateTime.UtcNow,
                CreateUserId = userId
            };

            await _context.Addresses.AddAsync(address, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AddressDto
            {
                Id = address.Id,
                UserId = _service.UserId,
                Region = address.Region,
                City = address.City,
                Street = address.Street,
                PostalCode = address.PostalCode,
                IsDefault = address.IsDefault,
            };
        }
    }
}
