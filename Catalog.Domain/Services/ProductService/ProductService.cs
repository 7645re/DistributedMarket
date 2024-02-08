using Catalog.Domain.Dto;
using Catalog.Domain.Mappers;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;
using Catalog.Domain.Repositories.ProductCategory;
using Catalog.Domain.UnitOfWork;

namespace Catalog.Domain.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
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
        using (_unitOfWork)
        {
            await _unitOfWork.BeginTransactionAsync();
            var productEntity = product.ToProductEntity();
            _unitOfWork.ProductRepository.Add(productEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var productsCategories = product
                .Categories
                .Select(c => new ProductEntityCategoryEntity
                {
                    ProductId = productEntity.Id,
                    CategoryId = c.Id,
                });
            _unitOfWork.ProductCategoryRepository.AddRange(productsCategories);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return productEntity.ToProduct();
        }
    }

    // TODO: Add Unit of Work
    public async Task DeleteProductByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        await _productRepository.DeleteProductByIdAsync(id, cancellationToken);
    }
}