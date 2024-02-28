using Catalog.Domain.Dto.Category;
using Shared.DiagnosticContext;

namespace Catalog.Domain.Services.CategoryService;

public class CategoryServiceDecorator : ICategoryService
{
    private readonly ICategoryService _categoryService;
    
    private readonly IDiagnosticContextStorage _diagnosticContextStorage;

    public CategoryServiceDecorator(
        ICategoryService categoryService,
        IDiagnosticContextStorage diagnosticContextStorage)
    {
        _categoryService = categoryService;
        _diagnosticContextStorage = diagnosticContextStorage;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure(nameof(GetCategoriesAsync)))
            return await _categoryService.GetCategoriesAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure(nameof(GetCategoryByIdAsync)))
            return await _categoryService.GetCategoryByIdAsync(id, cancellationToken); 
    }

    public async Task<Category> CreateCategoryAsync(CategoryCreate categoryCreate, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure(nameof(CreateCategoryAsync)))
            return await _categoryService.CreateCategoryAsync(categoryCreate, cancellationToken);
    }

    public async Task<Category> UpdateCategoryAsync(CategoryUpdate categoryUpdate, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure(nameof(UpdateCategoryAsync)))
            return await _categoryService.UpdateCategoryAsync(categoryUpdate, cancellationToken);
    }

    public async Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure(nameof(DeleteCategoryByIdAsync)))
            await _categoryService.DeleteCategoryByIdAsync(id, cancellationToken); 
    }
}