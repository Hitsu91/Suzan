
using Suzan.Domain.DTOs.Category;

namespace Suzan.Application.Services.CategoryService;

public interface ICategoryService
{
    Task<List<CategoryGetDto>> GetAll();
    Task<CategoryGetDto> Add(CategoryAddDto category);
    Task<CategoryGetDto> Update(Guid id, CategoryUpdateDto category);
    Task<CategoryGetDto> Delete(Guid id);
}