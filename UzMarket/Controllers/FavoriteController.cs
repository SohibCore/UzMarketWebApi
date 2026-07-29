using MediatR;
using Microsoft.AspNetCore.Mvc;
using UzMarket.RepositoryLayer.Dtos.FavoriteDtos;
using UzMarket.ServiceLayer.Services.FavoriteServices.Dtos;
using UzMarket.ServiceLayer.Services.FavoriteServices.Queries;
using UzMarket.ServiceLayer.Services.FavoriteServices.Commands;
using Microsoft.AspNetCore.Authorization;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FavoriteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FavoriteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] FavoriteFilterDto filter)
        {
            var result = await _mediator.Send(new GetListQuery(filter));
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] int Id)
        {
            var result = await _mediator.Send(new GetByIdQuery(Id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFavoriteDlDto dto)
        {
            var result = await _mediator.Send(new CreateFavoriteCommand(dto));
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            var result = await _mediator.Send(new DeleteFavoriteCommand(Id));
            return Ok(result);
        }
    }
}
