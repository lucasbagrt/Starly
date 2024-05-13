using Businesses.Domain.Dtos;
using Businesses.Domain.Filters;
using Businesses.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Starly.Domain.Utilities;

namespace Businesses.API.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] CategoryFilter filter)
    {
        var categories = await _categoryService.GetAllAsync(filter);
        if (categories == null)
            return NotFound();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetById(id);
        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto categoryDto)
    {
        var response = await _categoryService.Create(categoryDto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _categoryService.Delete(id);
        return Ok(response);
    }

    [HttpPut]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public async Task<IActionResult> Update([FromBody] CategoryDto categoryDto)
    {
        var response = await _categoryService.Update(categoryDto);
        return Ok(response);
    }
}