using UzMarket.RepositoryLayer.Dtos.UserDtos;

namespace UzMarket.ServiceLayer.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserListDto>> GetListAsync(UserFilterDto filter);
        Task<UserDto> GetAsync(long id);
        Task<UserDto> CreateAsync(CreateUserDlDto dto, CancellationToken cancellation);
        Task<UserDto> UpdateAsync(UpdateUserDlDto dto, CancellationToken cancellation);
        Task<string> DeleteAsync(long id, CancellationToken cancellation);
    }
}