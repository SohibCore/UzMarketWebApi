using UzMarket.RepositoryLayer.Dtos.CartDtos;

namespace UzMarket.ServiceLayer.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartListDto>> GetListAsync(CartFilterDto filter);

        Task<CartDto> GetAsync(long Id);

        Task<CartDto> CreateAsync(CreateCartDlDto dto, CancellationToken cancellationToken);

        Task<CartDto> UpdateAsync(UpdateCartDlDto dto, CancellationToken cancellationToken);

        Task<string> DeleteAsync(long Id, CancellationToken cancellationToken);
    }
}
