using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suzan.Application.Services.RecipeService;
using Suzan.Domain.DTOs.Recipe;
using Suzan.Domain.Exceptions;
using Suzan.Domain.Model;

namespace Suzan.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class RecipesController : ControllerBase
{
    private readonly ILogger<RecipesController> _logger;
    private readonly IRecipeService _service;

    public RecipesController(IRecipeService service, ILogger<RecipesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResponse<RecipeGetDto>>> GetAll([FromQuery] PaginationFilter filter)
    {
        var result = await _service.GetAll(filter);
        return Ok(result);
    }

    [HttpGet("my-recipes")]
    public async Task<ActionResult<PagedResponse<RecipeGetDto>>> GetAllMine([FromQuery] PaginationFilter filter)
    {
        var result = await _service.GetAllMine(filter);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<RecipeGetDto>> GetById(Guid id)
    {
        var result = await _service.GetById(id);
        if (result is null) return NotFound($"Cannot find recipe with id {id}");
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RecipeGetDto>> Add([FromBody] RecipeAddDto newRecipe)
    {
        try
        {
            var result = await _service.Add(newRecipe);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (ModelValidationException ex)
        {
            return StatusCode(ex.StatusCode, ex.Errors);
        }
    }
}