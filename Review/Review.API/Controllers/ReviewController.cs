using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Review.Domain.Dtos;
using Review.Domain.Filters;
using Review.Domain.Interfaces.Services;
using Starly.Domain.Extensions;

namespace Review.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] ReviewFilter filter)
    {
        var categories = await _reviewService.GetAllAsync(filter, this.GetAccessToken());
        if (categories == null)
            return NotFound();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var review = await _reviewService.GetById(id, this.GetAccessToken());
        if (review == null)
            return NotFound();

        return Ok(review);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto reviewDto)
    {
        var response = await _reviewService.Create(reviewDto, this.GetUserIdLogged(), this.GetAccessToken());
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _reviewService.Delete(id, this.GetUserIdLogged());
        return Ok(response);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateReviewDto updateReviewDto)
    {
        var response = await _reviewService.Update(updateReviewDto, this.GetUserIdLogged(), this.GetAccessToken());
        return Ok(response);
    }

    [HttpPost("UploadPhoto")]
    [Authorize]
    public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoDto uploadPhotoDto)
    {
        var response = await _reviewService.UploadPhoto(uploadPhotoDto, this.GetUserIdLogged());
        return Ok(response);
    }
}
