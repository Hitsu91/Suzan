using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Suzan.Application.Data;
using Suzan.Domain.DTOs.Category;
using Suzan.Domain.Exceptions;
using Suzan.Domain.Model;

namespace Suzan.Application.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly DataContext _ctx;
    private readonly IMapper _mapper;

    public CategoryService(DataContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }

    public async Task<List<CategoryGetDto>> GetAll()
    {
        return await _ctx.Categories.Select(c => _mapper.Map<CategoryGetDto>(c)).ToListAsync();
    }

    public async Task<CategoryGetDto> Add(CategoryAddDto category)
    {
        var newCategory = _mapper.Map<Category>(category);

        var addedCategory = await _ctx.Categories.AddAsync(newCategory);

        await _ctx.SaveChangesAsync();

        return _mapper.Map<CategoryGetDto>(addedCategory.Entity);
    }

    public async Task<CategoryGetDto> Update(Guid id, CategoryUpdateDto updatedCategory)
    {
        if (!await _ctx.Categories.AnyAsync(c => c.Id == id))
            throw new ModelValidationException(
                "Category id",
                StatusCodes.Status404NotFound,
                nameof(id),
                $"Category with id {id}, does not exists"
            );

        var category = _mapper.Map<Category>(updatedCategory);
        category.Id = id;
        _ctx.Categories.Update(category);
        await _ctx.SaveChangesAsync();

        return _mapper.Map<CategoryGetDto>(category);
    }

    public Task<CategoryGetDto> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}