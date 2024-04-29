using Starly.Domain.Dtos.Default;
using Starly.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Customer.Domain.Interfaces.Services;
using Customer.Domain.Dtos.Auth;
using Starly.Domain.Utilities;

namespace Customer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    [SwaggerOperation(Summary = "Create a new user")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var register = await _authService.RegisterAsync(registerDto);
        return Ok(register);
    }

    [HttpPost]
    [Route("login")]
    [SwaggerOperation(Summary = "Get user token")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(LoginResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var login = await _authService.LoginAsync(loginDto);
        return Ok(login);
    }

    [HttpPost]
    [Authorize]
    [Route("refresh-token")]
    [SwaggerOperation(Summary = "Refresh user token")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(LoginResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (refreshTokenDto is null)
            return BadRequest("Parametros invalidos!");

        var refresh = await _authService.RefreshTokenAsync(this.GetAccessToken(), refreshTokenDto.RefreshToken, User.Identity.Name);
        return Ok(refresh);
    }

    [HttpPost]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    [Route("revoke/{username}")]
    [SwaggerOperation(Summary = "Revoke user token")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> Revoke(string username)
    {
        var revoke = await _authService.RevokeAsync(username);
        return Ok(revoke);
    }
}
