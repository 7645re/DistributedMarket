using Catalog.Domain.Dto;
using Catalog.Domain.Mappers;
using Catalog.Domain.Models;
using Catalog.Domain.UnitOfWork;

namespace Catalog.Domain.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
    {
        var products = await _unitOfWork
            .ProductRepository
            .GetAllAsync(cancellationToken);
        return products.ToProducts();
    }

    public async Task<IEnumerable<Product>> GetProductWithCategoriesAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _unitOfWork
            .ProductRepository
            .GetProductByIdWithCategoriesAsync(id, cancellationToken);
        return product.ToProducts();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var productsByCategories = await _unitOfWork
            .ProductCategoryRepository
            .GetProductsCategoriesByCategoryIdAsync(id, cancellationToken);
        return productsByCategories.ToProducts();
    }

    public async Task<Product> CreateProductAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        var categoriesEntities = await _unitOfWork
            .CategoryRepository
            .GetCategoriesByIdsAsync(
                product.Categories.Select(c => c.Id),
                cancellationToken);
        if (!categoriesEntities.Any())
            throw new Exception("Categories not found");

        var productEntity = product.ToProductEntity();
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        _unitOfWork.ProductRepository.Add(productEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var productsCategories = product
            .Categories
            .Select(c => new ProductEntityCategoryEntity
            {
                ProductId = productEntity.Id,
                CategoryId = c.Id
            });
        _unitOfWork.ProductCategoryRepository.AddRange(productsCategories);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        return productEntity.ToProduct();
    }

    public async Task DeleteProductByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var existedProduct = await _unitOfWork.ProductRepository.GetProductByIdAsync(id, cancellationToken);
        if (existedProduct is null)
            throw new ArgumentException($"Product with id {id} not found");
        
        _unitOfWork.ProductRepository.Delete(existedProduct);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}