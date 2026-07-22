using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.AddressDtos;
using UzMarket.ServiceLayer.Security;

namespace UzMarket.ServiceLayer.MediatorServices.AddressServices.Commands
{
    public record UpdateAddressCommand(UpdateAddressDlDto dto) : IRequest<long>;

    public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, long>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public UpdateAddressHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<long> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == request.dto.Id);

            if (address == null)
                throw new Exception($"{request.dto.Id} - not found");

            if (!string.IsNullOrWhiteSpace(request.dto.Region))
                address.Region = request.dto.Region;

            if (!string.IsNullOrWhiteSpace(request.dto.City))
                address.City = request.dto.City;

            if (!string.IsNullOrWhiteSpace(request.dto.Street))
                address.Street = request.dto.Street;

            if (!string.IsNullOrWhiteSpace(request.dto.PostalCode))
                address.PostalCode = request.dto.PostalCode;

            if (address.IsDefault != request.dto.IsDefault)
                address.IsDefault = request.dto.IsDefault;

            address.ModifiedAt = DateTime.UtcNow;
            address.ModifiedUserId = _service.UserId;

            await _context.SaveChangesAsync(cancellationToken);

            return address.Id;
        }
    }
}
