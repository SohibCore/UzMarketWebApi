using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.ServiceLayer.MediatorServices.UserServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.UserServices.Queries;
using UzMarket.ServiceLayer.MediatorServices.UserServices.Commands;

namespace UzMarket.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] UserFilterDto filter)
        {
            var result = await _mediator.Send(new GetListQuery(filter));

            if (result is null || result.Count == 0)
                return NotFound($"User was not found.");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var result = await _mediator.Send(new GetByIdQuery(id));

            if (result is null)
                return NotFound($"User not found: {id}");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDlDto dto, CancellationToken cancel)
        {
            var result = await _mediator.Send(new CreateUserCommand(dto), cancel);

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateUserDlDto dto, CancellationToken cancel)
        {
            var result = await _mediator.Send(new UpdateUserCommand(dto), cancel);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancel)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id), cancel);

            return Ok(result);
        }
    }
}
