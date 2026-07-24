using MediatR;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;

namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Commands
{
    public record UpdateOrderCommand(UpdateOrderDlDto dto) : IRequest<bool>;
}
