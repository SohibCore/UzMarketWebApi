using MediatR;
using UzMarket.Core;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.MediatorServices.UserServices.Commands
{
    public record UpdateUserCommand(UpdateUserDlDto dto) : IRequest<bool>;

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public UpdateUserHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellation)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.dto.Id && x.StatusId != (int)StatusIdConst.DELETED, cancellation);

            if (user == null)
                throw new Exception($"User not found : {request.dto.Id}");

            if (request.dto.UserName != null && request.dto.UserName != user.UserName)
                user.UserName = request.dto.UserName;

            var userName = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.UserName == request.dto.UserName, cancellation);

            if (userName)
                throw new Exception($"This UserName ({user.UserName}) is already exists.");

            if (request.dto.Password != null && request.dto.Password != user.HashPassword)
                user.HashPassword = request.dto.Password;

            var password = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.HashPassword == request.dto.Password, cancellation);

            if (password)
                throw new Exception($"This Password ({user.HashPassword}) is already exists.");

            if (request.dto.FullName != null && request.dto.FullName != user.FullName)
                user.FullName = request.dto.FullName;
            if (request.dto.ShortName != null && request.dto.ShortName != user.ShortName)
                user.ShortName = request.dto.ShortName;
            if (request.dto.Pinfl != null && request.dto.Pinfl != user.Pinfl)
                user.Pinfl = request.dto.Pinfl;

            var pinfl = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.Pinfl == request.dto.Pinfl, cancellation);

            if (pinfl)
                throw new Exception($"This PINFL ({user.Pinfl}) is already exists.");

            if (request.dto.Email != null && request.dto.Email != user.Email)
                user.Email = request.dto.Email;

            if (request.dto.Address != null && request.dto.Address != user.Address)
                user.Address = request.dto.Address;

            if (request.dto.PhoneNumber != null && request.dto.PhoneNumber != user.PhoneNumber)
                user.PhoneNumber = request.dto.PhoneNumber;

            var phoneNumber = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.PhoneNumber == request.dto.PhoneNumber, cancellation);

            if (phoneNumber)
                throw new Exception($"This PhoneNumber ({user.PhoneNumber}) is already exists.");

            if (request.dto.DateOfBirth != null && request.dto.DateOfBirth != user.DateOfBirth)
                user.DateOfBirth = request.dto.DateOfBirth;

            if (request.dto.PassportSeries != null && request.dto.PassportSeries != user.PassportSeries)
                user.PassportSeries = request.dto.PassportSeries;

            var passportSeries = await _context.Users.AnyAsync(x => x.StatusId != (int)StatusIdConst.DELETED && x.PassportSeries == request.dto.PassportSeries, cancellation);

            if (passportSeries)
                throw new Exception($"This Passport Series ({user.PassportSeries}) is already exists.");

            user.ModifiedAt = DateTime.UtcNow;
            user.ModifiedUserId = _service.UserId;

            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}