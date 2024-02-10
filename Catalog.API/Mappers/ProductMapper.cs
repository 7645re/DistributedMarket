using Catalog.API.Dto.Requests;
using Catalog.API.Dto.Requests.Product;
using Catalog.Domain.Dto;

namespace Catalog.API.Mappers;

public static class ProductMapper
{
    public static Product ToProduct(this ProductCreate productCreate)
    {
        return new Product
        {
            Name = productCreate.Name,
            Price = productCreate.Price,
            Description = productCreate.Description,
            Categories = productCreate.Categories.ToCategories()
        };
    }

    public static IEnumerable<Category> ToCategories(this IEnumerable<int> categoriesIds)
    {
        return categoriesIds.Select(ci => new Category { Id = ci });
    }

    public static Product ToProduct(this ProductUpdate productUpdate, int id)
    {
        return new Product
        {
            Id = id,
            Name = productUpdate.Name,
            Price = productUpdate.Price,
            Description = productUpdate.Description,
            Categories = productUpdate.Categories.ToCategories()
        };
    }
}