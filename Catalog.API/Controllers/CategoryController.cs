using Catalog.API.Dto.Category;
using Catalog.API.Mappers;
using Catalog.Domain.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoriesAsync(cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CategoryCreateRequest categoryCreateRequest,
        CancellationToken cancellationToken)
    {
        var category = await _categoryService.CreateCategoryAsync(
            categoryCreateRequest.ToCategoryCreate(),
            cancellationToken);
        return Ok(category.ToCategoryCreateResponse());
    }
    
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateCategoryById(
        int id,
        [FromBody] CategoryUpdateRequest categoryUpdateRequest,
        CancellationToken cancellationToken)
    {
        var category = await _categoryService.UpdateCategoryAsync(
            categoryUpdateRequest.ToCategoryUpdate(id),
            cancellationToken);
        return Ok(category.ToCategoryUpdateResponse());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategoryById(
        int id,
        CancellationToken cancellationToken)
    {
        await _categoryService.DeleteCategoryByIdAsync(id, cancellationToken);
        return Ok();
    }
}