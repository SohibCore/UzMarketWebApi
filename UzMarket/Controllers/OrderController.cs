using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UzMarket.RepositoryLayer.Dtos.OrderDtos;
using UzMarket.ServiceLayer.Services.OrderServices;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] OrderFilterDto filter)
        {
            var result = await _orderService.GetListAsync(filter);

            if (result is null || result.Count == 0)
                return NotFound($"Order was not found.");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var result = await _orderService.GetAsync(Id);

            if (result is null)
                return NotFound($"Order not found: {Id}");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDlDto dto, CancellationToken cancellation)
        {
            var result = await _orderService.CreateAsync(dto, cancellation);

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDlDto dto, CancellationToken cancellation)
        {
            var result = await _orderService.UpdateAsync(dto, cancellation);

            if (result is null)
                return NotFound($"Order not found : {dto.Id}");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long Id, CancellationToken cancellation)
        {
            var result = await _orderService.DeleteAsync(Id, cancellation);

            if (result is null)
                return NotFound($"Order not found: {Id}");

            return Ok(result);
        }
    }
}
