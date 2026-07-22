using UzMarket.RepositoryLayer.Dtos.AuthDtos;
using UzMarket.RepositoryLayer.Dtos.UserDtos;

namespace UzMarket.ServiceLayer.Services.AuthServices
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(CreateUserDlDto dto, CancellationToken cancellationToken);
        Task<AuthResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
    }
}
