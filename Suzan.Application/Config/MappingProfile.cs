using AutoMapper;
using Suzan.Domain.DTOs.Category;
using Suzan.Domain.DTOs.Recipe;
using Suzan.Domain.DTOs.User;
using Suzan.Domain.Model;

namespace Suzan.Application.Config;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CATEGORY //
        CreateMap<CategoryAddDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
        CreateMap<Category, CategoryGetDto>();

        // RECIPE //
        CreateMap<RecipeAddDto, Recipe>();
        CreateMap<RecipeUpdateDto, Recipe>();
        CreateMap<Recipe, RecipeGetDto>();

        // USER //
        CreateMap<User, UserProfileDto>();
        CreateMap<User, AuthorDto>();
    }
}