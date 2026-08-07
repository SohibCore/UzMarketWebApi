using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UzMarket.Core.Common;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.Common.Commands;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Commands;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.ProductServices.Queries;

namespace UzMarket.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProductDto>>> GetProducts
            (
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null
            )
        {
            var result = await _mediator.Send(new ProductPaginatedListQuery(pageNumber, pageSize, search));
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDlDto dto)
        {
            var result = await _mediator.Send(new CreateProductCommand(dto));
            return Ok(result);
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateProductDlDto dto)
        {
            var result = await _mediator.Send(new UpdateProductCommand(dto));
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(Id));
            return Ok(result);
        }
    }
}
