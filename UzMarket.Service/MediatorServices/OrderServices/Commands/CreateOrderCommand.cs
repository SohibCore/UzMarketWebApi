using MediatR;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;

namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Commands
{
    public record CreateOrderCommand(CreateOrderDlDto dto) : IRequest<bool>;
}
