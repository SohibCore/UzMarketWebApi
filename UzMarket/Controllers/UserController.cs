using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.ServiceLayer.Services.UserServices;

namespace UzMarket.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] UserFilterDto filter)
        {
            var result = await _service.GetListAsync(filter);

            if (result is null || result.Count == 0)
                return NotFound($"User was not found.");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var result = await _service.GetAsync(id);

            if (result is null)
                return NotFound($"User not found: {id}");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDlDto dto, CancellationToken cancellation)
        {
            var result = await _service.CreateAsync(dto, cancellation);

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateUserDlDto dto, CancellationToken cancellation)
        {
            var result = await _service.UpdateAsync(dto, cancellation);

            if (result is null)
                return NotFound($"User not found : {dto.Id}");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellation)
        {
            var result = await _service.DeleteAsync(id, cancellation);

            if (result is null)
                return NotFound($"User not found : {id}");

            return Ok(result);
        }
    }
}
