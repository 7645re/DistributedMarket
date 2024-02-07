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
        var productsByCategories = await _productCategoryRepository
            .GetProductsCategoriesByCategoryIdAsync(id, cancellationToken);
        return productsByCategories.ToProducts();
    }

    public async Task<Product> CreateProductAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        var createdProduct = await _productRepository.AddAsync(product.ToProductEntity(), cancellationToken);
        return createdProduct.ToProduct();
    }

    public async Task DeleteProductByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        await _productRepository.DeleteProductByIdAsync(id, cancellationToken);
    }
}