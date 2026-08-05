using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.UserDtos;

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

            var userName = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.UserName == request.dto.UserName, cancellation);

            if (userName)
                throw new Exception($"This UserName ({user.UserName}) is already exists.");

            var password = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.HashPassword == request.dto.Password, cancellation);

            if (password)
                throw new Exception($"This Password ({user.HashPassword}) is already exists.");

            var pinfl = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.Pinfl == request.dto.Pinfl, cancellation);

            if (pinfl)
                throw new Exception($"This PINFL ({user.Pinfl}) is already exists.");

            var phoneNumber = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.PhoneNumber == request.dto.PhoneNumber, cancellation);

            if (phoneNumber)
                throw new Exception($"This PhoneNumber ({user.PhoneNumber}) is already exists.");

            var passportSeries = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.PassportSeries == request.dto.PassportSeries, cancellation);

            if (passportSeries)
                throw new Exception($"This Passport Series ({user.PassportSeries}) is already exists.");

            await _context.Users.AddAsync(user, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return true;
        }
    }
}
