using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;
using UzMarket.ServiceLayer.MediatorServices.OrderServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.OrderServices.Queries;
using UzMarket.ServiceLayer.MediatorServices.OrderServices.Commands;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] OrderFilterDto filter)
        {
            var result = await _mediator.Send(new GetListQuery(filter));

            if (result is null || result.Count == 0)
                return NotFound($"Order was not found.");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetByIdQuery(Id));

            if (result is null)
                return NotFound($"Order not found: {Id}");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDlDto dto)
        {
            var result = await _mediator.Send(new CreateOrderCommand(dto));

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDlDto dto)
        {
            var result = await _mediator.Send(new UpdateOrderCommand(dto));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long Id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new DeleteOrderCommand(Id));

            return Ok(result);
        }
    }
}
