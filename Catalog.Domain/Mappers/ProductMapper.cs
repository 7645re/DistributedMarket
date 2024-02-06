using Catalog.Domain.Dto;
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
            Description = productEntity.Description,
            Categories = productEntity.Categories.ToCategories()
        };
    }

    public static IEnumerable<Product> ToProducts(this IEnumerable<ProductEntity> productEntities)
    {
        return productEntities.Select(pe => pe.ToProduct());
    }

    public static IEnumerable<Product> ToProducts(this IEnumerable<ProductEntityCategoryEntity> productsCategories)
    {
        return productsCategories.Select(pc => pc.Product).ToProducts();
    }
}