using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suzan.Application.Services.CategoryService;
using Suzan.Domain.DTOs.Category;
using Suzan.Domain.Exceptions;
using Suzan.Domain.Model;

namespace Suzan.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = nameof(Role.Admin))]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<List<CategoryGetDto>>> GetAll()
    {
        var result = await _categoryService.GetAll();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> AddCategory(CategoryAddDto newCategory)
    {
        var result = await _categoryService.Add(newCategory);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Category>> UpdateCategory(Guid id, CategoryUpdateDto updatedCategory)
    {
        try
        {
            var result = await _categoryService.Update(id, updatedCategory);
            return Accepted(result);
        }
        catch (ModelValidationException e)
        {
            return StatusCode(e.StatusCode, e.Errors);
        }
    }
}

