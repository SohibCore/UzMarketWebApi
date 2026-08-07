using UzMarket.RepositoryLayer.Dtos.AuthDtos;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.ServiceLayer.Security.RegisterServices.Commands;

namespace UzMarket.ServiceLayer.Security.AuthServices
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(CreateUserDlDto dto, CancellationToken cancellationToken);
        Task<AuthResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
        Task<AuthResult> VerifyEmailAsync(VerifyEmailCommand command);
    }
}
