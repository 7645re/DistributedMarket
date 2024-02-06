using Catalog.Domain.Dto;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategoryRepository;

namespace Catalog.Domain.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductService(
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return products.ToProducts();
    }

    public async Task<IEnumerable<Product>> GetProductWithCategoriesAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdWithCategoriesAsync(id, cancellationToken);
        return product.ToProducts();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var productsByCategories = await _productCategoryRepository.GetProductsCategoriesByCategoryIdAsync(id, cancellationToken);
        return productsByCategories.ToProducts();
    }
}