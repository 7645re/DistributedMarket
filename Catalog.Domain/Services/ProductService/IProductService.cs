using Catalog.Domain.Dto;

namespace Catalog.Domain.Services.ProductService;

public interface IProductService
{
    Task<Product> GetProductWithCategoriesByIdAsync(
        int id,
        CancellationToken cancellationToken);
    
    Task DeleteProductByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<Product> CreateProductAsync(
        Product product,
        CancellationToken cancellationToken);

    Task<Product> UpdateProductByIdAsync(Product product,
        CancellationToken cancellationToken);
}