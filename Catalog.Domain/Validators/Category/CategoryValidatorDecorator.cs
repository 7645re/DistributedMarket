using Catalog.Domain.Dto.Category;
using Catalog.Domain.Models;
using Shared.DiagnosticContext;

namespace Catalog.Domain.Validators.Category;

public class CategoryValidatorDecorator : ICategoryValidator
{
    private readonly ICategoryValidator _categoryValidator;
    
    private readonly IDiagnosticContextStorage _diagnosticContextStorage;

    public CategoryValidatorDecorator(
        ICategoryValidator categoryValidator,
        IDiagnosticContextStorage diagnosticContextStorage)
    {
        _categoryValidator = categoryValidator;
        _diagnosticContextStorage = diagnosticContextStorage;
    }

    public async Task ValidateAsync(CategoryCreate categoryCreate, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(CategoryValidatorDecorator)}.{nameof(ValidateAsync)}"))
            await _categoryValidator.ValidateAsync(categoryCreate, cancellationToken);
    }

    public async Task ValidateAsync(
        CategoryEntity categoryEntity,
        CategoryUpdate categoryUpdate,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(CategoryValidatorDecorator)}.{nameof(ValidateAsync)}"))
            await _categoryValidator.ValidateAsync(categoryEntity, categoryUpdate, cancellationToken);
    }
}