using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UzMarket.RepositoryLayer.Dtos.AuthDtos;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.ServiceLayer.Services.AuthServices;

namespace UzMarket.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUserDlDto dto, CancellationToken cancel)
        {
            var result = await _service.RegisterAsync(dto, cancel);

            // Bu qator brauzerga "Set-Cookie: .AspNetCore.Cookies=..." yuboradi
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.ClaimsPrincipal);

            return Ok(new { result.UserId, result.UserName, result.FullName });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancel)
        {
            var result = await _service.LoginAsync(dto, cancel);

            // Bu qator brauzerga "Set-Cookie: .AspNetCore.Cookies=..." yuboradi
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
    }
}
