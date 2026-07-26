using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.UserDtos;

namespace UzMarket.ServiceLayer.MediatorServices.UserServices.Commands
{
    public record UpdateUserCommand(UpdateUserDlDto dto) : IRequest<bool>;

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly AppDbContext _context;
        public UpdateUserHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellation)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.dto.Id, cancellation);

            if (user == null)
                throw new Exception($"User not found : {request.dto.Id}");

            if(request.dto.UserName != null && request.dto.UserName != user.UserName)
                user.UserName = request.dto.UserName;
            if(request.dto.Password != null && request.dto.Password != user.HashPassword)
                user.HashPassword = request.dto.Password;
            if(request.dto.FullName != null && request.dto.FullName != user.FullName)
                user.FullName = request.dto.FullName;
            if(request.dto.ShortName != null && request.dto.ShortName != user.ShortName)
                user.ShortName = request.dto.ShortName;
            if(request.dto.Pinfl != null && request.dto.Pinfl != user.Pinfl)
                user.Pinfl = request.dto.Pinfl;
            if(request.dto.Email != null && request.dto.Email != user.Email)
                user.Email = request.dto.Email;
            if(request.dto.Address != null && request.dto.Address != user.Address)
                user.Address = request.dto.Address;
            if(request.dto.PhoneNumber != null && request.dto.PhoneNumber != user.PhoneNumber)
                user.PhoneNumber = request.dto.PhoneNumber;
            if(request.dto.DateOfBirth != null && request.dto.DateOfBirth != user.DateOfBirth)
                user.DateOfBirth = request.dto.DateOfBirth;
            if(request.dto.PassportSeries != null && request.dto.PassportSeries != user.PassportSeries)
                user.PassportSeries = request.dto.PassportSeries;

            user.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}