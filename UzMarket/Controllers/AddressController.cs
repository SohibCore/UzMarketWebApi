using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.AddressDtos;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Commands;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Dtos;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Queries;

namespace UzMarket.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;
        public AddressController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] AddressFilterDto dto)
        {
            var result = await _mediator.Send(new GetListQuery(dto));

            if (result == null || result.Count == 0)
                return NotFound($"Not found information about Address");

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] int Id)
        {
            var result = await _mediator.Send(new GetByIdQuery(Id));

            if (result == null)
                return NotFound($"Not found information about Address");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressDlDto dto)
        {
            var result = await _mediator.Send(new CreateAddressCommand(dto));

            if (result == null)
                return NotFound($"Not found information about Address");

            return Ok(result.Id);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateAddressDlDto dto)
        {
            var result = await _mediator.Send(new UpdateAddressCommand(dto));

            var validation = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (validation is null)
                return NotFound($"Not found information about Address : {dto.Id}");

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _mediator.Send(new DeleteAddressCommand(Id));

            if (result == null)
                return NotFound($"Not found information about Address : {Id}");

            return Ok(result);
        }
    }
}
