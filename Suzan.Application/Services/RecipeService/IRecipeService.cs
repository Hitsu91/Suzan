using Suzan.Domain.DTOs.Recipe;
using Suzan.Domain.Model;

namespace Suzan.Application.Services.RecipeService;

public interface IRecipeService
{
    Task<PagedResponse<RecipeGetDto>> GetAll(PaginationFilter filter);
    Task<PagedResponse<RecipeGetDto>> GetAllMine(PaginationFilter filter);
    Task<PagedResponse<RecipeGetDto>> GetAllByCategory(PaginationFilter filter, Guid categoryId);
    Task<RecipeGetDto?> GetById(Guid id);
    Task<RecipeGetDto> Add(RecipeAddDto newRecipe);
    Task<RecipeGetDto?> Update(Guid id, RecipeUpdateDto updatedRecipe);
    Task<RecipeGetDto?> Delete(Guid id);
}