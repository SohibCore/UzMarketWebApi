using MediatR;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.Services.FavoriteServices.Commands
{
    public record DeleteFavoriteCommand(long Id) : IRequest<bool>;

    public class DeleteFavoriteHandler : IRequestHandler<DeleteFavoriteCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;

        public DeleteFavoriteHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<bool> Handle(DeleteFavoriteCommand request, CancellationToken cancellation)
        {
            var favorite = await _context.Favorites
                .SingleOrDefaultAsync(x => x.Id == request.Id && x.UserId == _service.UserId && x.StatusId != Core.StatusIdConst.DELETED, cancellation);

            if (favorite == null)
                throw new NotFoundException($"Favorite not found : {request.Id}");

            favorite.StatusId = Core.StatusIdConst.DELETED;
            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}