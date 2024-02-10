using Catalog.API.Dto.Requests.Category;
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
    public async Task<IActionResult> GetCategoryById(
        int id,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategoryByIdAsync(
            id,
            cancellationToken);
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CategoryCreate categoryCreate,
        CancellationToken cancellationToken)
    {
        var category = await _categoryService.CreateCategoryAsync(
            categoryCreate.ToCategory(),
            cancellationToken);
        return Ok(category);
    }
    
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateCategoryById(
        int id,
        [FromBody] CategoryUpdate categoryUpdate,
        CancellationToken cancellationToken)
    {
        var category = await _categoryService.UpdateCategoryAsync(
            categoryUpdate.ToCategory(id),
            cancellationToken);
        return Ok(category);
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