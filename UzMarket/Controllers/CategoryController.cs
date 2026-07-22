using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UzMarket.RepositoryLayer.Dtos.CategoryDtos;
using UzMarket.ServiceLayer.MediatorServices.CategoryServices.Commands;
using UzMarket.ServiceLayer.MediatorServices.CategoryServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.CategoryServices.Queries;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] CategoryFilterDto dto)
        {
            var result = await _mediator.Send(new GetListQuery(dto));

            if (result == null || result.Count == 0)
                return NotFound($"Not found information about Category");

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] int Id)
        {
            var result = await _mediator.Send(new GetByIdQuery(Id));

            if (result == null || result.Id != Id)
                return NotFound($"Not found information about Category : {Id}");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDlDto dto)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(dto));

            if (result == null)
                return NotFound($"Not found information about Category");

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryDlDto dto)
        {
            var result = await _mediator.Send(new UpdateCategoryCommand(dto));
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(Id));

            if (result == null)
                return NotFound($"Not found information about Category : {Id}");

            return Ok(result);
        }
    }
}
