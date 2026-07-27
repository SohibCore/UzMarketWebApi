using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UzMarket.RepositoryLayer.Dtos.CartDtos;
using UzMarket.ServiceLayer.MediatorServices.CartServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.CartServices.Queries;
using UzMarket.ServiceLayer.MediatorServices.CartServices.Commands;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] CartFilterDto filter)
        {
            var result = await _mediator.Send(new GetListQuery(filter));

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetByIdQuery(Id));

            if (result is null)
                return NotFound($"Cart not found : {Id}");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCartDlDto dto)
        {
            var result = await _mediator.Send(new CreateCartCommand(dto));

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCartDlDto dto)
        {
            var result = await _mediator.Send(new UpdateCartCommand(dto));

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] long Id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteCartCommand(Id));

            return Ok(result);
        }
    }
}
