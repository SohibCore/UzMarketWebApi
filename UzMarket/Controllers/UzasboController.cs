using Microsoft.AspNetCore.Mvc;
using UzMarket.ServiceLayer.Services.Integration.Interfaces;

namespace UzMarket.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UzasboController : ControllerBase
    {
        private readonly IUzasboService _service;
        public UzasboController(IUzasboService service)
        {
            _service = service;
        }
        [HttpGet("{pinfl}")]
        public async Task<IActionResult> Get([FromRoute] string pinfl, CancellationToken cancellation)
        {
            var result = await _service.GetPersonInfoAsync(pinfl, cancellation);
            return Ok(result);
        }
    }
}
