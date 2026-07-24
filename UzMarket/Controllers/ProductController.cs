using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Queries;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Commands;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ProductFilterDto filter)
        {
            var result = await _mediator.Send(new GetListQuery(filter));

            if (result is null)
                return NotFound($"Product not found : {result}");

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetByIdQuery(Id));

            if (result is null)
                return NotFound($"Product not found : {Id}");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDlDto dto)
        {
            var result = await _mediator.Send(new CreateProductCommand(dto));
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateProductDlDto dto)
        {
            var result = await _mediator.Send(new UpdateProductCommand(dto));
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(Id));
            return Ok(result);
        }
    }
}
