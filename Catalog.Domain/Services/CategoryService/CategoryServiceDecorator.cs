using Catalog.Domain.Dto.Category;
using Shared.DiagnosticContext;

namespace Catalog.Domain.Services.CategoryService;

public class CategoryServiceDecorator : ICategoryService
{
    private readonly ICategoryService _categoryService;
    
    private readonly IDiagnosticContext _diagnosticContext;

    public CategoryServiceDecorator(
        ICategoryService categoryService,
        IDiagnosticContext diagnosticContext)
    {
        _categoryService = categoryService;
        _diagnosticContext = diagnosticContext;
    }

    public async Task<List<Category>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryService)}.{nameof(GetAllPagedAsync)}"))
            return await _categoryService.GetAllPagedAsync(page, pageSize, cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryService)}.{nameof(GetCategoryByIdAsync)}"))
            return await _categoryService.GetCategoryByIdAsync(id, cancellationToken); 
    }

    public async Task<Category> CreateCategoryAsync(CategoryCreate categoryCreate, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryService)}.{nameof(CreateCategoryAsync)}"))
            return await _categoryService.CreateCategoryAsync(categoryCreate, cancellationToken);
    }

    public async Task<Category> UpdateCategoryAsync(CategoryUpdate categoryUpdate, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryService)}.{nameof(UpdateCategoryAsync)}"))
            return await _categoryService.UpdateCategoryAsync(categoryUpdate, cancellationToken);
    }

    public async Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryService)}.{nameof(DeleteCategoryByIdAsync)}"))
            await _categoryService.DeleteCategoryByIdAsync(id, cancellationToken); 
    }
}