using Catalog.Domain.Dto;
using Catalog.Domain.Dto.Category;
using Catalog.Domain.Dto.Product;
using Catalog.Domain.Models;

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
    
    public static Product ToProduct(this ProductEntity productEntity, IEnumerable<int> categoriesIds)
    {
        return new Product
        {
            Id = productEntity.Id,
            Name = productEntity.Name,
            Price = productEntity.Price,
            Count = productEntity.Count,
            Description = productEntity.Description,
            Categories = categoriesIds.Select(id => new Category {Id = id})
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
}