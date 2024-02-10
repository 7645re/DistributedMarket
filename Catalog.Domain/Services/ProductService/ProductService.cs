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
        var productEntity = await _unitOfWork.ProductRepository.GetByIdAsync(id, cancellationToken);
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
        }, cancellationToken);

        return productEntity.ToProduct(productCreate.Categories);
    }

    public async Task<Product> UpdateProductAsync(
        ProductUpdate productUpdate, CancellationToken cancellationToken)
    {
        var productEntityById = await _unitOfWork.ProductRepository.GetByIdAsync(
            productUpdate.Id, cancellationToken);
        if (productEntityById is null)
            throw new InvalidOperationException($"A product with such an ID: {productUpdate.Id} does not exist");

        await _productValidator.ValidateAsync(productUpdate, productEntityById, cancellationToken);
        
        UpdateChangedFields(productUpdate, productEntityById);
        var productsCategoriesEntitiesForAdd = 
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _unitOfWork.ProductRepository.Update(productEntityById);
            
        }, cancellationToken);
        
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

    public Task DeleteProductByIdAsync(
        int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}