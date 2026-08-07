using MediatR;
using UzMarket.Core;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.AuthDtos;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.ServiceLayer.Security.RegisterServices.Commands;

namespace UzMarket.ServiceLayer.Security.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public AuthService(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<AuthResult> RegisterAsync(CreateUserDlDto dto, CancellationToken cancellationToken)
        {
            var userNameTaken = await _context.Users
                .AnyAsync(x => x.UserName == dto.UserName, cancellationToken);

            if (userNameTaken)
                throw new Exception($"'{dto.UserName}' foydalanuvchi nomi allaqachon band.");

            var user = new User
            {
                UserName = dto.UserName,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                ShortName = dto.ShortName,
                Pinfl = dto.Pinfl,
                Email = dto.Email,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                PassportSeries = dto.PassportSeries,

                StatusId = (int)StatusIdConst.CREATED,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResult
            {
                UserId = user.Id,
                UserName = user.UserName,
                ClaimsPrincipal = BuildClaimsPrincipal(user)
            };
        }

        public async Task<AuthResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == dto.UserName, cancellationToken);

            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.HashPassword))
                throw new Exception("UserName yoki parol noto'g'ri.");

            if (user.StatusId == (int)StatusIdConst.DELETED)
                throw new Exception("Ushbu hisob o'chirilgan.");

            return new AuthResult
            {
                UserId = user.Id,
                UserName = user.UserName,
                ClaimsPrincipal = BuildClaimsPrincipal(user)
            };
        }

        private ClaimsPrincipal BuildClaimsPrincipal(User user)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.UserName),
                new ("FullName", user.FullName),
                new ("ShortName", user.ShortName)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            return new ClaimsPrincipal(identity);
        }

        public async Task<AuthResult> VerifyEmailAsync(VerifyEmailCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
