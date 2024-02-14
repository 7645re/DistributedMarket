using Catalog.Domain.Dto.Product;
using Catalog.Domain.Mappers;
using Catalog.Domain.Models;
using Catalog.Domain.UnitOfWork;
using Catalog.Domain.Validators.Product;

namespace Catalog.Domain.Services.ProductService;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IProductValidator _productValidator;

    public ProductService(
        IUnitOfWork unitOfWork,
        IProductValidator productValidator)
    {
        _unitOfWork = unitOfWork;
        _productValidator = productValidator;
    }

    public async Task<Product> GetProductByIdAsync(
        int id, CancellationToken cancellationToken)
    {
        var productEntity = await _unitOfWork.ProductRepository.GetByIdWithCategoriesAsync(id, cancellationToken);
        if (productEntity is null)
            throw new InvalidOperationException($"Product with id {id} not found");
        
        return productEntity.ToProduct();
    }

    public async Task<IEnumerable<Product>> GetProductByCategoryIdAsync(
        int categoryId, CancellationToken cancellationToken)
    {
        var productsEntities = await _unitOfWork.ProductRepository
            .GetByCategoryIdAsync(categoryId, cancellationToken);
        return productsEntities.ToProducts();
    }

    public async Task<IEnumerable<Product>> GetProductByCategoriesIdsAsync(
        IEnumerable<int> categoriesIds, CancellationToken cancellationToken)
    {
        var productsEntities = await _unitOfWork.ProductRepository
            .GetByCategoriesIdsAsync(categoriesIds, cancellationToken);
        return productsEntities.ToProducts();
    }

    public async Task<Product> CreateProductAsync(
        ProductCreate productCreate, CancellationToken cancellationToken)
    {
        await _productValidator.ValidateAsync(productCreate, cancellationToken);

        var productEntity = productCreate.ToProductEntityWithoutCategories();
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var savedEntity = _unitOfWork.ProductRepository.Add(productEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productCategoryEntity = productCreate
                .Categories
                .Select(id => new ProductEntityCategoryEntity
                {
                    ProductId = savedEntity.Id,
                    CategoryId = id
                })
                .ToList();
            _unitOfWork.ProductCategoryRepository.AddRange(productCategoryEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);

        return productEntity.ToProduct(productCreate.Categories);
    }

    public async Task<Product> UpdateProductAsync(
        ProductUpdate productUpdate, CancellationToken cancellationToken)
    {
        var productEntityById = await _unitOfWork.ProductRepository.GetByIdWithCategoriesAsync(
            productUpdate.Id, cancellationToken);
        if (productEntityById is null)
            throw new InvalidOperationException($"A product with such an ID: {productUpdate.Id} does not exist");

        await _productValidator.ValidateAsync(productUpdate, productEntityById, cancellationToken);
        
        UpdateChangedFields(productUpdate, productEntityById);
        int[]? productCategoriesForDelete = null;
        ProductEntityCategoryEntity[]? productCategoriesForAdd = null;
        if (productUpdate.Categories is not null)
        {
            var oldCategoriesIds = productEntityById.Categories.Select(c => c.Id).ToArray();
            productCategoriesForAdd = productUpdate
                .Categories
                .Except(oldCategoriesIds)
                .Select(id => new ProductEntityCategoryEntity
                {
                    ProductId = productEntityById.Id,
                    CategoryId = id
                })
                .ToArray();
            productCategoriesForDelete = oldCategoriesIds.Except(productUpdate.Categories).ToArray();
        }

       
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var productEntityWithoutCategories = productEntityById;
            productEntityWithoutCategories.Categories = Array.Empty<CategoryEntity>();
            _unitOfWork.ProductRepository.Update(productEntityWithoutCategories);
            if (productCategoriesForAdd is not null)
                _unitOfWork.ProductCategoryRepository.AddRange(productCategoriesForAdd);
            if (productCategoriesForDelete is not null)
                _unitOfWork.ProductCategoryRepository
                    .DeleteByProductIdAndCategoriesIds(
                        productEntityById.Id, productCategoriesForDelete, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);

        return productEntityById.ToProduct(productUpdate.Categories);
        
        void UpdateChangedFields(ProductUpdate productUpdateForUpdate, ProductEntity productEntityForUpdate)
        {
            if (productUpdateForUpdate.Name is not null)
                productEntityForUpdate.Name = productUpdateForUpdate.Name;

            if (productUpdateForUpdate.Price is not null)
                productEntityForUpdate.Price = productUpdateForUpdate.Price.Value;

            if (productUpdateForUpdate.Count is not null)
                productEntityForUpdate.Count = productUpdateForUpdate.Count.Value;
            
            if (productUpdateForUpdate.Description is not null)
                productEntityForUpdate.Description = productUpdateForUpdate.Description;
        }
    }

    public async Task DeleteProductByIdAsync(
        int id, CancellationToken cancellationToken)
    {
        var existingProduct = _unitOfWork.ProductRepository.GetByIdAsync(id, cancellationToken);
        if (existingProduct is null)
            throw new InvalidOperationException($"A product with such an ID: {id} does not exist");

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _unitOfWork.ProductCategoryRepository.DeleteByProductId(id, cancellationToken);
            _unitOfWork.ProductRepository.DeleteById(id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
    }
}