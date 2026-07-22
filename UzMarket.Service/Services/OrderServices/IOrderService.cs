using UzMarket.RepositoryLayer.Dtos.OrderDtos;

namespace UzMarket.ServiceLayer.Services.OrderServices
{
    public interface IOrderService
    {
        Task<List<OrderListDto>> GetListAsync(OrderFilterDto filter);

        Task<OrderDto> GetAsync(long Id);

        Task<OrderDto> CreateAsync(CreateOrderDlDto dto, CancellationToken cancellation);

        Task<OrderDto> UpdateAsync(UpdateOrderDlDto dto, CancellationToken cancellation);

        Task<string> DeleteAsync(long Id, CancellationToken cancellation);
    }
}
