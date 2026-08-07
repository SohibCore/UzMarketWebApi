using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.ServiceLayer.Security.RegisterServices.Interfaces;
using OpenQA.Selenium;
using UzMarket.ServiceLayer.ExternalServices.RegisterServices.Dtos;
using UzMarket.ServiceLayer.Services.Integration.Interfaces;

namespace UzMarket.ServiceLayer.Security.RegisterServices.Commands
{
    public record RegisterCommand(CreateUserDlDto dto, string pinfl) : IRequest<RegisterDto>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterDto>
    {
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IUzasboService _uzasboService;

        public RegisterCommandHandler(AppDbContext context, IEmailSender emailSender, IUzasboService uzasboService)
        {
            _context = context;
            _emailSender = emailSender;
            _uzasboService = uzasboService;
        }

        public async Task<RegisterDto> Handle(RegisterCommand request, CancellationToken ct)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.dto.Email, ct);

            if (existingUser is not null)
                throw new Exception("Bu email allaqachon ro'yxatdan o'tgan");

            var code = new Random().Next(100000, 999999).ToString();

            var pending = await _context.PendingRegistrations
                .FirstOrDefaultAsync(p => p.Email == request.dto.Email, ct);

            var personInfo = await _uzasboService.GetPersonInfoAsync(request.pinfl, ct);

            if (pending is not null)
            {
                pending.Password = request.dto.Password;
                pending.FullName = personInfo.Name;
                pending.ShortName = personInfo.ShortName;
                pending.Pinfl = personInfo.PersonalNum;
                pending.PhoneNumber = request.dto.PhoneNumber;
                pending.Address = personInfo.Address;
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

            return new RegisterDto
            {
                FulleName = personInfo.Name,
                Address = personInfo.Address,
                Pinfl = personInfo.PersonalNum,
            };
        }
    }
}
