using Starly.Domain.Dtos.Default;
using Starly.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Customer.Domain.Interfaces.Services;
using Customer.Domain.Dtos.User;
using Customer.Domain.Filters;
using Starly.Domain.Utilities;
using Starly.Domain.Dtos;

namespace Customer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    [SwaggerOperation(Summary = "Get all users")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IList<UserResponseDto>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetAll([FromQuery] UserFilter filter)
    {
        var users = await _userService.GetAllAsync(filter);
        if (users is null)
            return NotFound();

        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    [SwaggerOperation(Summary = "Get user by id")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UserResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("GetUserInfo/{id}")]
    [Authorize]
    [SwaggerOperation(Summary = "Get user info")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UserInfoDto))]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetUserInfo(int id)
    {
        var user = await _userService.GetUserInfoAsync(id);
        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut]
    [Authorize]
    [SwaggerOperation(Summary = "Update user")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserDto)
    {
        var response = await _userService.UpdateAsync(updateUserDto, this.GetUserIdLogged());
        return Ok(response);
    }

    [HttpPut("Password")]
    [Authorize]
    [SwaggerOperation(Summary = "Change password")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordDto updateUserPasswordDto)
    {
        var response = await _userService.UpdatePasswordAsync(updateUserPasswordDto, this.GetUserIdLogged());
        return Ok(response);
    }


    [HttpDelete]
    [Authorize]
    [SwaggerOperation(Summary = "Delete user")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> Delete()
    {
        var response = await _userService.DeleteAsync(this.GetUserIdLogged());
        return Ok(response);
    }

    [HttpPost("UploadPhoto")]
    [Authorize]
    [SwaggerOperation(Summary = "Upload photo")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> UploadPhoto(IFormFile file)
    {
        var response = await _userService.UploadPhotoAsync(file, this.GetUserIdLogged());
        return Ok(response);
    }
}