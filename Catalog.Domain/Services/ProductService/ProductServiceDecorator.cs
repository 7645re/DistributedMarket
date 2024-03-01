using Catalog.Domain.Dto.Product;
using Catalog.Domain.Repositories;
using Shared.DiagnosticContext;

namespace Catalog.Domain.Services.ProductService;

public class ProductServiceDecorator : IProductService
{
    private readonly IProductService _productService;
    
    private readonly IDiagnosticContextStorage _diagnosticContextStorage;


    public ProductServiceDecorator(
        IProductService productService,
        IDiagnosticContextStorage diagnosticContextStorage)
    {
        _productService = productService;
        _diagnosticContextStorage = diagnosticContextStorage;
    }

    public Task<List<Product>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(ProductService)}.{nameof(GetAllPagedAsync)}"))
            return _productService.GetAllPagedAsync(page, pageSize, cancellationToken);
    }

    public async Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(ProductService)}.{nameof(GetProductByIdAsync)}"))
            return await _productService.GetProductByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(ProductService)}.{nameof(GetProductByCategoryIdAsync)}"))
            return await _productService.GetProductByCategoryIdAsync(categoryId, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductByCategoriesIdsAsync(IEnumerable<int> categoriesIds, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(ProductService)}.{nameof(GetProductByCategoriesIdsAsync)}"))
            return await _productService.GetProductByCategoriesIdsAsync(categoriesIds, cancellationToken);
    }

    public async Task<Product> CreateProductAsync(ProductCreate productCreate, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(ProductService)}.{nameof(CreateProductAsync)}"))
            return await _productService.CreateProductAsync(productCreate, cancellationToken);
    }

    public async Task<Product> UpdateProductAsync(ProductUpdate productUpdate, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(ProductService)}.{nameof(UpdateProductAsync)}"))
            return await _productService.UpdateProductAsync(productUpdate, cancellationToken);
    }

    public async Task DeleteProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        using (_diagnosticContextStorage.Measure($"{nameof(ProductService)}.{nameof(DeleteProductByIdAsync)}"))
            await _productService.DeleteProductByIdAsync(id, cancellationToken);
    }
}