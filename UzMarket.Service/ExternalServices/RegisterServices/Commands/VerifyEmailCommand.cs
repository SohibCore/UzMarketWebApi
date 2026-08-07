using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.AuthDtos;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.ServiceLayer.Security.AuthServices;

namespace UzMarket.ServiceLayer.Security.RegisterServices.Commands
{
    public record VerifyEmailCommand(string Email, string Code) : IRequest<AuthResult>;

    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, AuthResult>
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public VerifyEmailCommandHandler(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<AuthResult> Handle(VerifyEmailCommand request, CancellationToken ct)
        {
            var pending = await _context.PendingRegistrations
                .FirstOrDefaultAsync(p => p.Email == request.Email, ct);

            if (pending is null)
                throw new NotFoundException("Ro'yxatdan o'tish topilmadi, qaytadan urinib ko'ring");

            if (pending.ExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("Kod muddati tugagan");

            if (pending.Code != request.Code)
            {
                pending.AttemptCount++;
                await _context.SaveChangesAsync(ct);
                throw new BadRequestException("Kod noto'g'ri");
            }

            var dto = new CreateUserDlDto
            {
                Email = pending.Email,
                Password = pending.Password,
                FullName = pending.FullName,
                UserName = pending.UserName,
                ShortName = pending.ShortName,
                Pinfl = pending.Pinfl,
                Address = pending.Address,
                PhoneNumber = pending.PhoneNumber,
                DateOfBirth = pending.DateOfBirth,
                PassportSeries = pending.PassportSeries,
            };

            var authResult = await _authService.RegisterAsync(dto, ct);

            _context.PendingRegistrations.Remove(pending);
            await _context.SaveChangesAsync(ct);

            return authResult;
        }
    }
}
