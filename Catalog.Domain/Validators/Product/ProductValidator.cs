using Catalog.Domain.Dto.Product;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories.Category;
using Catalog.Domain.Repositories.Product;

namespace Catalog.Domain.Validators.Product;

public class ProductValidator : IProductValidator
{
    private readonly IProductRepository _productRepository;

    private readonly ICategoryRepository _categoryRepository;

    public ProductValidator(IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task ValidateAsync(ProductCreate productCreate, CancellationToken cancellationToken)
    {
        CountShouldBeNotNegative(productCreate.Count);
        PriceShouldBeNotNegative(productCreate.Price);
        NameLengthShouldBeMoreZero(productCreate.Name);
        await NameShouldBeUniqueAsync(productCreate.Name, cancellationToken);
        await CategoriesShouldBeExistsAsync(productCreate.Categories.ToArray(), cancellationToken);
    }
    
    public async Task ValidateAsync(
        ProductUpdate productUpdate, ProductEntity productEntity, CancellationToken cancellationToken)
    {
        AtLeastOneFieldMustChanged(productEntity, productUpdate);
        
        if (productUpdate.Price is not null)
            PriceShouldBeNotNegative(productUpdate.Price.Value);
        
        if (productUpdate.Count is not null)
            CountShouldBeNotNegative(productUpdate.Count.Value);
        
        if (productUpdate.Name is not null)
            NameLengthShouldBeMoreZero(productUpdate.Name);

        if (productUpdate.Categories is not null)
            await CategoriesShouldBeExistsAsync(productUpdate.Categories.ToArray(), cancellationToken);
    }

    private void AtLeastOneFieldMustChanged(ProductEntity productEntity, ProductUpdate productUpdate)
    {
        var productEntityForCompare = new ProductForCompare(productEntity.Name, productEntity.Price,
            productEntity.Description, productEntity.Count, productEntity.Categories.Select(c => c.Id).ToArray());
        var productUpdateForCompare = new ProductForCompare(productUpdate.Name, productUpdate.Price,
            productUpdate.Description, productUpdate.Count, productUpdate.Categories.ToArray());

        if (productEntityForCompare.Equals(productUpdateForCompare))
            throw new InvalidOperationException("No changes");
    }

    private async Task CategoriesShouldBeExistsAsync(int[] categoriesIds, CancellationToken cancellationToken)
    {
        var neededCategories = await _categoryRepository.GetByIdsAsync(categoriesIds, cancellationToken);
        if (neededCategories.Count < categoriesIds.Length)
        {
            var exceptCategoriesId = categoriesIds.Except(neededCategories.Select(c => c.Id));
            throw new InvalidOperationException($"The specified product categories with " +
                                                $"ids: {string.Join(", ", exceptCategoriesId)} do not exist");
        }        
    }

    private async Task NameShouldBeUniqueAsync(
        string name, CancellationToken cancellationToken, int? exclusiveId = null)
    {
        var productWithSameName = await _productRepository.GetByNameAsync(name, cancellationToken);
        if (productWithSameName is not null)
        {
            if (exclusiveId is not null && productWithSameName.Id == exclusiveId)
                throw new InvalidOperationException($"Product already has such a name");

            throw new InvalidOperationException($"A product with that name: {name} already exists");
        }
    }

    private void CountShouldBeNotNegative(int count)
    {
        if (count < 0)
            throw new InvalidOperationException("The quantity of the product cannot be less than zero");
    }

    private void PriceShouldBeNotNegative(decimal price)
    {
        if (price < 0)
            throw new InvalidOperationException("The price of the product cannot be less than zero");
    }

    private void NameLengthShouldBeMoreZero(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("The product name cannot be empty or consist" +
                                                " of only whitespace characters.");
    }
}

record ProductForCompare(string? Name, decimal? Price, string? Description, int? Count, int[] CategoriesIds); 