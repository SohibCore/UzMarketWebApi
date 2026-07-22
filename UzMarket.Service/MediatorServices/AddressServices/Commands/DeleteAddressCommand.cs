using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security;

namespace UzMarket.ServiceLayer.MediatorServices.AddressServices.Commands
{
    public record DeleteAddressCommand(int Id) : IRequest<string>;

    public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, string>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public DeleteAddressHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<string> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == _service.UserId);

            if (address == null)
                throw new Exception($"{request.Id} not found");

            address.StatusId = (int)StatusIdConst.DELETED;
            await _context.SaveChangesAsync(cancellationToken);

            return $"Order with ID {request.Id} has been deleted successfully.";
        }
    }
}
