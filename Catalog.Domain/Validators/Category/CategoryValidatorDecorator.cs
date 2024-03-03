using Catalog.Domain.Dto.Category;
using Catalog.Domain.Models;
using Shared.DiagnosticContext;

namespace Catalog.Domain.Validators.Category;

public class CategoryValidatorDecorator : ICategoryValidator
{
    private readonly ICategoryValidator _categoryValidator;
    
    private readonly IDiagnosticContext _diagnosticContext;

    public CategoryValidatorDecorator(
        ICategoryValidator categoryValidator,
        IDiagnosticContext diagnosticContext)
    {
        _categoryValidator = categoryValidator;
        _diagnosticContext = diagnosticContext;
    }

    public async Task ValidateAsync(CategoryCreate categoryCreate, CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryValidatorDecorator)}.{nameof(ValidateAsync)}"))
            await _categoryValidator.ValidateAsync(categoryCreate, cancellationToken);
    }

    public async Task ValidateAsync(
        CategoryEntity categoryEntity,
        CategoryUpdate categoryUpdate,
        CancellationToken cancellationToken)
    {
        using (_diagnosticContext.Measure($"{nameof(CategoryValidatorDecorator)}.{nameof(ValidateAsync)}"))
            await _categoryValidator.ValidateAsync(categoryEntity, categoryUpdate, cancellationToken);
    }
}