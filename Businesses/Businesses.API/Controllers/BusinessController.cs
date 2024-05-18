using Businesses.Domain.Dtos;
using Businesses.Domain.Filters;
using Businesses.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Starly.Domain.Extensions;
using Starly.Domain.Utilities;

namespace Businesses.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BusinessController : Controller
{
    private readonly IBusinessService _businessService;

    public BusinessController(IBusinessService businessService)
    {
        _businessService = businessService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] BusinessFilter filter)
    {
        var businesses = await _businessService.GetAllAsync(filter, this.GetAccessToken());
        if (businesses == null)
            return NotFound();

        return Ok(businesses);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var business = await _businessService.GetById(id, this.GetAccessToken());
        if (business == null)
            return NotFound();

        return Ok(business);
    }

    [HttpPost]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public async Task<IActionResult> Create([FromBody] CreateBusinessDto createBusinessDto)
    {
        var response = await _businessService.Create(createBusinessDto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _businessService.Delete(id);
        return Ok(response);
    }

    [HttpPut]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public async Task<IActionResult> Update([FromBody] UpdateBusinessDto updateBusinessDto)
    {
        var response = await _businessService.Update(updateBusinessDto);
        return Ok(response);
    }

    [HttpPost("UploadPhoto")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public async Task<IActionResult> UploadPhotos([FromForm] UploadPhotoDto uploadPhotoDto)
    {
        var response = await _businessService.UploadPhoto(uploadPhotoDto);
        return Ok(response);
    }

    [HttpPut("DeletePhoto/{id}")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public async Task<IActionResult> DeletePhoto(int id)
    {
        var response = await _businessService.DeletePhoto(id);
        return Ok(response);
    }

    [HttpGet("ExistsById/{id}")]
    [Authorize]
    public async Task<IActionResult> ExistsById(int id)
    {
        var response = await _businessService.ExistsById(id);
        return Ok(response);
    }
}
