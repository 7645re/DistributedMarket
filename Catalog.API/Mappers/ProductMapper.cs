using Catalog.API.Dto.Requests;
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

    public static Category ToCategory(this CategoryCreate categoryCreate)
    {
        return new Category
        {
            Name = categoryCreate.Name
        };
    }

    public static IEnumerable<Category> ToCategories(this IEnumerable<CategoryCreate> categoryCreates)
    {
        return categoryCreates.Select(cc => cc.ToCategory());
    }
}