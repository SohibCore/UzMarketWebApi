using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.UserServices.Dtos;

namespace UzMarket.ServiceLayer.MediatorServices.UserServices.Queries
{
    public record GetByIdQuery(long Id) : IRequest<UserDto>;

    public class GetByIdHandler : IRequestHandler<GetByIdQuery, UserDto>
    {
        private readonly AppDbContext _context;
        public GetByIdHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserDto> Handle(GetByIdQuery request, CancellationToken cancellation)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellation);

            if (user == null)
                throw new Exception($"User not found : {request.Id}");

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                ShortName = user.ShortName,
                Pinfl = user.Pinfl,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                PassportSeries = user.PassportSeries,
                Email = user.Email,
                StatusId = user.StatusId
            };
        }
    }
}
