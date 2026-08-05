using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using UzMarket.RepositoryLayer.Dtos.AuthDtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using UzMarket.ServiceLayer.Security.AuthServices;
using UzMarket.ServiceLayer.Services.RegisterServices.Commands;
using MediatR;

namespace UzMarket.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IMediator _mediator;

        public AuthController(IAuthService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancel)
        {
            await _mediator.Send(command, cancel);
            return Ok(new { message = "Tasdiqlash kodi emailga yuborildi" });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancel)
        {
            var result = await _service.LoginAsync(dto, cancel);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.ClaimsPrincipal);
            return Ok(new { result.UserId, result.UserName, result.FullName });
        }

        [HttpGet]
        public IActionResult Me()
        {
            if (User.Identity?.IsAuthenticated != true)
                return Unauthorized();

            return Ok(new
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                UserName = User.Identity!.Name,
                FullName = User.FindFirst("FullName")?.Value
            });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                result.ClaimsPrincipal);
            return Ok(new { userId = result.UserId, userName = result.UserName, fullName = result.FullName });
        }
    }
}