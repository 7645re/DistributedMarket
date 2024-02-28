using Catalog.Domain.Dto.Category;
using Catalog.Domain.Dto.Product;
using Catalog.Domain.Models;
using Shared.Messaging.Events.Product;

namespace Catalog.Domain.Mappers;

public static class ProductMapper
{
    public static Product ToProduct(this ProductEntity productEntity)
    {
        return new Product
        {
            Id = productEntity.Id,
            Name = productEntity.Name,
            Price = productEntity.Price,
            Count = productEntity.Count,
            Description = productEntity.Description,
            Categories = productEntity.Categories.ToCategories()
        };
    }
    
    public static Product ToProduct(this ProductEntity productEntity, IEnumerable<int>? categoriesIds)
    {
        return new Product
        {
            Id = productEntity.Id,
            Name = productEntity.Name,
            Price = productEntity.Price,
            Count = productEntity.Count,
            Description = productEntity.Description,
            Categories = categoriesIds is null ? Array.Empty<Category>() 
                : categoriesIds.Select(id => new Category {Id = id})
        };
    }

    public static ProductEntity ToProductEntityWithoutCategories(this ProductCreate productCreate)
    {
        return new ProductEntity
        {
            Name = productCreate.Name,
            Price = productCreate.Price,
            Description = productCreate.Description,
            Count = productCreate.Count
        };
    }

    public static IEnumerable<Product> ToProducts(this IEnumerable<ProductEntity> productEntities)
    {
        return productEntities.Select(pe => pe.ToProduct());
    }

    public static IEnumerable<Product> ToProducts(this IEnumerable<ProductEntityCategoryEntity>? productsCategories)
    {
        return productsCategories is null ? Array.Empty<Product>() 
            :  productsCategories.Select(pc => pc.Product).ToProducts();
    }

    public static ProductEntity ToProductEntity(this ProductUpdate productUpdate)
    {
        return new ProductEntity
        {
            Id = productUpdate.Id,
            Name = productUpdate.Name,
            Price = productUpdate.Price.Value,
            Description = null,
            Count = 0,
            Categories = null
        };
    }

    public static ProductCreateEvent ToProductCreateEvent(this ProductEntity productCreate)
    {
        return new ProductCreateEvent
        {
            Id = productCreate.Id,
            Name = productCreate.Name,
            Price = productCreate.Price,
            Description = productCreate.Description,
            Count = productCreate.Count,
            Timestamp = DateTime.UtcNow
        };
    }
    
    public static ProductUpdateEvent  ToProductUpdateEvent(this ProductEntity productEntity,
        ProductUpdate productUpdate)
    {
        return new ProductUpdateEvent
        {
            Id = productEntity.Id,
            OldName = productEntity.Name,
            NewName = productUpdate.Name,
            OldPrice = productEntity.Price,
            NewPrice = productUpdate.Price,
            OldCount = productEntity.Count,
            NewCount = productUpdate.Count,
            OldDescription = productEntity.Description,
            NewDescription = productUpdate.Description,
            OldCategories = productEntity.Categories.Select(c => c.Id).ToArray(),
            NewCategories = productUpdate.Categories,
            Timestamp = DateTime.UtcNow
        };
    }
}