using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;

namespace UzMarket.ServiceLayer.MediatorServices.UserServices.Commands
{
    public record DeleteUserCommand(long Id) : IRequest<bool>;

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly AppDbContext _context;
        public DeleteUserHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellation)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellation);

            if (user == null)
                throw new Exception($"User not found : {request.Id}");

            user.StatusId = (int)StatusIdConst.DELETED;
            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}
