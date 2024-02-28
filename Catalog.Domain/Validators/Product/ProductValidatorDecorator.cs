using Catalog.Domain.Dto.Product;
using Catalog.Domain.Models;
using Shared.DiagnosticContext;

namespace Catalog.Domain.Validators.Product;

public class ProductValidatorDecorator : IProductValidator
{
    private readonly IProductValidator _productValidator;
    private readonly IDiagnosticContextStorage _diagnosticContextStorage;

    public ProductValidatorDecorator(
        IProductValidator productValidator,
        IDiagnosticContextStorage diagnosticContextStorage)
    {
        _productValidator = productValidator;
        _diagnosticContextStorage = diagnosticContextStorage;
    }

    public async Task ValidateAsync(ProductCreate productCreate, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(ProductValidatorDecorator)}.{nameof(ValidateAsync)}"))
            await _productValidator.ValidateAsync(productCreate, cancellationToken);
    }

    public async Task ValidateAsync(ProductUpdate productUpdate, ProductEntity productEntity, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(ProductValidatorDecorator)}.{nameof(ValidateAsync)}"))
            await _productValidator.ValidateAsync(productUpdate, productEntity, cancellationToken);
    }
}