using Catalog.Domain.Dto;
using Catalog.Domain.Mappers;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategoryRepository;
using Catalog.Domain.Services.CategoryService;

namespace Catalog.Domain.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _categoryRepository = categoryRepository;
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
        var productCategoriesIds = product
            .Categories
            .Select(c => c.Id)
            .ToArray();
        if (productCategoriesIds.Any())
        {
            var existedCategoriesEntities = await _categoryRepository
                .GetCategoriesByIdsAsync(productCategoriesIds, cancellationToken);
            if (productCategoriesIds.Length != existedCategoriesEntities.Count)
                throw new ArgumentException("One of these product categories does not exist");

            product.Categories = existedCategoriesEntities.ToCategories();
        }
        
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