using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.AuthDtos;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.RepositoryLayer.Entity;

namespace UzMarket.ServiceLayer.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext context)
        {
            _context = context;
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
                FullName = user.FullName,
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
                FullName = user.FullName,
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
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            return new ClaimsPrincipal(identity);
        }
    }
}
