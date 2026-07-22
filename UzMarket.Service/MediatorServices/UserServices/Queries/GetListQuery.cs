using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.UserServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.UserServices.Queries.QueryObjects;

namespace UzMarket.ServiceLayer.MediatorServices.UserServices.Queries
{
    public record GetListQuery(UserFilterDto filter) : IRequest<List<UserListDto>>;

    public class GetListHandler : IRequestHandler<GetListQuery, List<UserListDto>>
    {
        private readonly AppDbContext _context;
        public GetListHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserListDto>> Handle(GetListQuery request, CancellationToken cancellation)
        {
            var users = await _context.Users
                .Select(user => new UserListDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    ShortName = user.ShortName,
                    Email = user.Email,
                    StatusId = user.StatusId
                }).SortFilter(request.filter)
                .ToListAsync(cancellation);

            return users;
        }
    }
}
