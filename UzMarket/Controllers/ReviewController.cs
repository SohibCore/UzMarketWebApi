using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UzMarket.RepositoryLayer.Dtos.ReviewDtos;
using UzMarket.ServiceLayer.Services.ReviewServices.Dtos;
using UzMarket.ServiceLayer.Services.ReviewServices.Queries;
using UzMarket.ServiceLayer.Services.ReviewServices.Commands;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ReviewFilterDto filter)
        {
            var result = await _mediator.Send(new GetListQuery(filter));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int Id)
        {
            var result = await _mediator.Send(new GetByIdQuery(Id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewDlDto dto)
        {
            var result = await _mediator.Send(new CreateReviewCommand(dto));
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateReviewDlDto dto)
        {
            var result = await _mediator.Send(new UpdateReviewCommand(dto));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var result = await _mediator.Send(new DeleteReviewCommand(Id));
            return Ok(result);
        }

    }
}
