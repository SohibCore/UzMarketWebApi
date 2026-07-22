using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UzMarket.RepositoryLayer.Dtos.ProductDtos;
using UzMarket.ServiceLayer.Services.ProductServices;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ProductFilterDto filter)
        {
            var result = await _service.GetListAsync(filter);

            if (result is null)
                return NotFound($"Product not found : {result}");

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var result = await _service.GetAsync(Id);

            if (result is null)
                return NotFound($"Product not found : {Id}");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDlDto dto, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(dto, cancellationToken);

            if (result is null)
                return NotFound($"Failed to create the product.");

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateProductDlDto dto, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(dto, cancellationToken);

            if (result is null)
                return NotFound($"Product not found : {dto.Id}");

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] long Id, CancellationToken cancellationToken)
        {
            var result = await _service.DeleteAsync(Id, cancellationToken);

            if (result is null)
                return NotFound($"Product not found : {Id}");

            return Ok(result);
        }
    }
}
