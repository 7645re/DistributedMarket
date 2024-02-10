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
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetProductWithCategoriesAsync(
        int id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> CreateProductAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        var productCategories = product.Categories;
        var categoriesEntities = (await _unitOfWork
            .CategoryRepository
            .GetByIdsAsync(
                product.Categories.Select(c => c.Id),
                cancellationToken)).ToArray();

        if (categoriesEntities.Length != productCategories.Count())
        {
            var exceptCategoriesIds = product
                .Categories
                .Select(c => c.Id)
                .Except(categoriesEntities.Select(c => c.Id))
                .ToArray();
            throw new InvalidOperationException(
                "Categories with ids " + string.Join(", ", exceptCategoriesIds) + " not found");
        }

        var productEntity = product.ToProductEntityWithoutCategories();
        await _unitOfWork.ExecuteInTransactionAsync(
            async () =>
            {
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
            },
            cancellationToken);

        return productEntity.ToProduct(categoriesEntities);
    }

    public async Task DeleteProductByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(id, cancellationToken);
        if (existingProduct is null)
            throw new InvalidOperationException($"Product with id {id} not found");

        await _unitOfWork.ExecuteInTransactionAsync(() =>
            {
                _unitOfWork.ProductRepository.Remove(existingProduct);
                _unitOfWork.ProductCategoryRepository.DeleteByProductId(id, cancellationToken);
                return Task.CompletedTask;
            }, cancellationToken);
    }

    public async Task UpdateProductByIdAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        var existingProductEntity = await _unitOfWork
            .ProductRepository
            .GetByIdAsync(product.Id, cancellationToken);
        if (existingProductEntity is null)
            throw new InvalidOperationException($"Product with id {product.Id} doesnt exist");

        var existingProduct = existingProductEntity.ToProduct();
        if (!IsUpdated(existingProduct, product))
            throw new InvalidOperationException("No changes");

        var productCategories = product.Categories.Select(c => c.Id).ToArray();
        var existingCategoriesIds = existingProduct.Categories.Select(c => c.Id).ToArray();
        var existingCategories = await _unitOfWork
            .CategoryRepository
            .GetByIdsAsync(productCategories, cancellationToken);
        if (existingCategories.Count() != product.Categories.Count())
            throw new InvalidOperationException("Categories not found");
        
        var categoriesForDeleteIds = existingCategoriesIds
            .Except(productCategories).ToArray();
        var categoriesForAddIds = productCategories
            .Except(existingCategoriesIds).ToArray();
        
        var productEntity = product.ToProductEntityWithoutCategories();
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _unitOfWork.ProductRepository.Update(productEntity);
            _unitOfWork.ProductCategoryRepository
                .DeleteByProductIdAndCategoriesIds(
                    product.Id,
                    categoriesForDeleteIds,
                    cancellationToken);
            _unitOfWork.ProductCategoryRepository
                .AddRange(categoriesForAddIds.Select(categoryId => new ProductEntityCategoryEntity
                {
                    ProductId = product.Id,
                    CategoryId = categoryId
                }));
        }, cancellationToken);
        
        bool IsUpdated(Product productFromDb, Product productFromClient)
        {
            var result = false;
            if (productFromDb.Name != productFromClient.Name)
                result = true;
            if (productFromDb.Description != productFromClient.Description)
                result = true;
            if (productFromDb.Price != productFromClient.Price)
                result = true;

            if (!(productFromDb.Categories.Sum(c => c.Id) == productFromClient.Categories.Sum(c => c.Id)
                  && productFromDb.Categories.Count() == productFromClient.Categories.Count()))
                result = true;

            return result;
        }
    }
}