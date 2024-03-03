using Catalog.API.Dto.Category;
using Catalog.API.Mappers;
using Catalog.Domain.Dto.Category;
using Catalog.Domain.Repositories;
using Catalog.Domain.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using Shared.DiagnosticContext;

namespace Catalog.API.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    
    private readonly IDiagnosticContext _diagnosticContext;

    public CategoryController(
        ICategoryService categoryService,
        IDiagnosticContext diagnosticContext)
    {
        _categoryService = categoryService;
        _diagnosticContext = diagnosticContext;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Category>>> GetCategories(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryController)}.{nameof(GetCategories)}"))
        {
            var products = await _categoryService.GetAllPagedAsync(page, pageSize, cancellationToken);
            return Ok(products);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryController)}.{nameof(GetCategoryById)}"))
        {
            var category = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
            return Ok(category);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CategoryCreateRequest categoryCreateRequest,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryController)}.{nameof(CreateCategory)}"))
        {
            var category = await _categoryService.CreateCategoryAsync(
                categoryCreateRequest.ToCategoryCreate(),
                cancellationToken);
            return Ok(category.ToCategoryCreateResponse());
        }
    }
    
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateCategoryById(
        int id,
        [FromBody] CategoryUpdateRequest categoryUpdateRequest,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryController)}.{nameof(UpdateCategoryById)}"))
        {
            var category = await _categoryService.UpdateCategoryAsync(
                categoryUpdateRequest.ToCategoryUpdate(id),
                cancellationToken);
            return Ok(category.ToCategoryUpdateResponse());
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategoryById(
        int id,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryController)}.{nameof(DeleteCategoryById)}"))
        {
            await _categoryService.DeleteCategoryByIdAsync(id, cancellationToken);
            return Ok();
        }
    }
}