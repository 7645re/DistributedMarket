using Catalog.Domain.Dto.Product;

namespace Catalog.Domain.Services.ProductService;

public interface IProductService
{
    Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetProductByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetProductByCategoriesIdsAsync(
        IEnumerable<int> categoriesIds, CancellationToken cancellationToken);

    Task<Product> CreateProductAsync(ProductCreate productCreate, CancellationToken cancellationToken);

    Task<Product> UpdateProductAsync(ProductUpdate productUpdate, CancellationToken cancellationToken);

    Task DeleteProductByIdAsync(int id, CancellationToken cancellationToken);
}