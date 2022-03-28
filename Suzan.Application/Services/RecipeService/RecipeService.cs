using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Suzan.Application.Data;
using Suzan.Application.Helpers;
using Suzan.Application.Services.IdentityService;
using Suzan.Domain.Model;
using Suzan.Domain.DTOs.Recipe;
using Suzan.Domain.Exceptions;

namespace Suzan.Application.Services.RecipeService;

public class RecipeService : IRecipeService
{
    private readonly DataContext _ctx;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;
    private readonly IQueryable<Recipe> _recipes;

    public RecipeService(DataContext ctx, IMapper mapper, IIdentityService identityService)
    {
        _ctx = ctx;
        _mapper = mapper;
        _identityService = identityService;
        _recipes = _ctx.Recipes
            .Include(r => r.Category)
            .Include(r => r.Author);
    }

    public async Task<PagedResponse<RecipeGetDto>> GetAll(PaginationFilter filter)
    {
        var recipes = await _recipes.Paginate(filter)
                .Select(r => _mapper.Map<RecipeGetDto>(r))
                .ToListAsync();
        var count = await _recipes.CountAsync();

        return PaginationHelper.CreatePagedResponse(recipes, filter, count);
    }

    public async Task<PagedResponse<RecipeGetDto>> GetAllMine(PaginationFilter filter)
    {
        var myRecipes = _recipes
                            .Where(r => r.Author != null && r.Author.Id == _identityService.GetUserId());

        var recipes = await myRecipes
                            .Paginate(filter)
                            .Select(r => _mapper.Map<RecipeGetDto>(r))
                            .ToListAsync();

        var count = await myRecipes.CountAsync();

        return PaginationHelper.CreatePagedResponse(recipes, filter, count);
    }

    public async Task<PagedResponse<RecipeGetDto>> GetAllByCategory(PaginationFilter filter, Guid categoryId)
    {
        var recipes = _recipes.Where(r => r.Category != null && r.Category.Id == categoryId);
        var data = await recipes.Paginate(filter)
                        .Select(r => _mapper.Map<RecipeGetDto>(r))
                        .ToListAsync();

        var count = await recipes.CountAsync();

        return PaginationHelper.CreatePagedResponse(data, filter, count);
    }

    public async Task<RecipeGetDto?> GetById(Guid id)
    {
        var result = await _recipes.FirstOrDefaultAsync(r => r.Id == id);
        return _mapper.Map<RecipeGetDto>(result);
    }

    public async Task<RecipeGetDto> Add(RecipeAddDto newRecipe)
    {
        var author = await _identityService.GetCurrentUser();
        var category = await _ctx.Categories.FindAsync(newRecipe.CategoryId);

        if (category is null)
        {
            throw new ModelValidationException(
                "Add Recipe Error",
                StatusCodes.Status404NotFound,
                nameof(category),
                $"Cannot find a category with id {newRecipe.CategoryId}");
        }

        var recipe = _mapper.Map<Recipe>(newRecipe);
        recipe.Author = author;
        recipe.Category = category;
        var result = await _ctx.Recipes.AddAsync(recipe);
        await _ctx.SaveChangesAsync();
        return _mapper.Map<RecipeGetDto>(result.Entity);
    }

    public async Task<RecipeGetDto?> Update(Guid id, RecipeUpdateDto updatedRecipe)
    {
        if (!await ExistsAsync(id))
        {
            throw new ModelValidationException(
                "Update Recipe Error",
                StatusCodes.Status404NotFound,
                nameof(id),
                $"Cannot find a recipe with id {id}");
        }

        var recipe = _mapper.Map<Recipe>(updatedRecipe);
        recipe.Id = id;
        _ctx.Recipes.Update(recipe);
        await _ctx.SaveChangesAsync();
        return _mapper.Map<RecipeGetDto>(recipe);
    }

    public async Task<RecipeGetDto?> Delete(Guid id)
    {
        var recipe = await _ctx.Recipes.FindAsync(id);

        if (recipe is null)
        {
            throw new ModelValidationException(
                "Update Recipe Error",
                StatusCodes.Status404NotFound,
                nameof(id),
                $"Cannot find a recipe with id {id}");
        }

        _ctx.Recipes.Remove(recipe);
        await _ctx.SaveChangesAsync();

        return _mapper.Map<RecipeGetDto>(recipe);
    }

    private async Task<bool> ExistsAsync(Guid id)
    {
        return !await _ctx.Recipes.AnyAsync(r => r.Id == id);
    }


}