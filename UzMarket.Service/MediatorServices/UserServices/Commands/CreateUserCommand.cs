using MediatR;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.RepositoryLayer.Entity;

namespace UzMarket.ServiceLayer.MediatorServices.UserServices.Commands
{
    public record CreateUserCommand(CreateUserDlDto dto) : IRequest<bool>;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly AppDbContext _context;
        public CreateUserHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellation)
        {
            var user = new User
            {
                UserName = request.dto.UserName,
                HashPassword = request.dto.Password,
                FullName = request.dto.FullName,
                ShortName = request.dto.ShortName,
                Pinfl = request.dto.Pinfl,
                Email = request.dto.Email,
                Address = request.dto.Address,
                PhoneNumber = request.dto.PhoneNumber,
                DateOfBirth = request.dto.DateOfBirth,
                PassportSeries = request.dto.PassportSeries,

                CreatedAt = DateTime.UtcNow,
            };

            await _context.Users.AddAsync(user, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return true;
        }
    }
}
