using Catalog.Domain.Dto;

namespace Catalog.Domain.Services.ProductService;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetProductWithCategoriesAsync(int id,
        CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task DeleteProductByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<Product> CreateProductAsync(
        Product product,
        CancellationToken cancellationToken);

    Task UpdateProductByIdAsync(
        Product product,
        CancellationToken cancellationToken);
}