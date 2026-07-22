using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UzMarket.RepositoryLayer.Dtos.CartDtos;
using UzMarket.ServiceLayer.Services.CartServices;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] CartFilterDto filter)
        {
            var result = await _service.GetListAsync(filter);

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var result = await _service.GetAsync(Id);

            if (result is null)
                return NotFound($"Cart not found : {Id}");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCartDlDto dto, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(dto, cancellationToken);

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCartDlDto dto, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(dto, cancellationToken);

            if (result is null)
                return NotFound($"Cart not found : {dto.Id}");

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] long Id, CancellationToken cancellationToken)
        {
            var result = await _service.DeleteAsync(Id, cancellationToken);

            if (result is null)
                return NotFound($"Cart not found : {Id}");

            return Ok(result);
        }
    }
}
