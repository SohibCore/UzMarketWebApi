using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Services.RegisterServices.Interfaces;
using UzMarket.RepositoryLayer.Dtos.UserDtos;

namespace UzMarket.ServiceLayer.Services.RegisterServices.Commands
{
    public record RegisterCommand(CreateUserDlDto dto) : IRequest<Unit>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Unit>
    {
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;

        public RegisterCommandHandler(AppDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken ct)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.dto.Email, ct);

            if (existingUser is not null)
                throw new Exception("Bu email allaqachon ro'yxatdan o'tgan");

            var code = new Random().Next(100000, 999999).ToString();

            var pending = await _context.PendingRegistrations
                .FirstOrDefaultAsync(p => p.Email == request.dto.Email, ct);

            if (pending is not null)
            {
                // qayta urinish — eski yozuvni yangilaymiz
                pending.Password = request.dto.Password;
                pending.FullName = request.dto.FullName;
                pending.ShortName = request.dto.ShortName;
                pending.Pinfl = request.dto.Pinfl;
                pending.PhoneNumber = request.dto.PhoneNumber;
                pending.Address = request.dto.Address;
                pending.DateOfBirth = request.dto.DateOfBirth;
                pending.PassportSeries = request.dto.PassportSeries;
                pending.UserName = request.dto.UserName;

                pending.Code = code;
                pending.ExpiresAt = DateTime.UtcNow.AddMinutes(2);
                pending.AttemptCount = 0;
            }
            else
            {
                _context.PendingRegistrations.Add(new PendingRegistration
                {
                    Password = request.dto.Password,
                    FullName = request.dto.FullName,
                    ShortName = request.dto.ShortName,
                    Pinfl = request.dto.Pinfl,
                    PhoneNumber = request.dto.PhoneNumber,
                    Address = request.dto.Address,
                    DateOfBirth = request.dto.DateOfBirth,
                    PassportSeries = request.dto.PassportSeries,
                    UserName = request.dto.UserName,
                    Email = request.dto.Email,
                    Code = code,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(2)
                });
            }

            await _context.SaveChangesAsync(ct);

            await _emailSender.SendAsync(request.dto.Email, "Tasdiqlash kodi",
                $"Sizning tasdiqlash kodingiz: {code}");

            return Unit.Value;
        }
    }
}
