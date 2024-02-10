using Catalog.API.Dto.Requests;
using Catalog.Domain.Dto;

namespace Catalog.API.Mappers;

public static class ProductMapper
{
    public static Product ToProduct(this ProductCreate productCreate)
    {
        return new Product(
            productCreate.Name,
            productCreate.Price,
            productCreate.Description,
            productCreate.Categories.ToCategories());
    }

    public static IEnumerable<Category> ToCategories(this IEnumerable<int> categoriesIds)
    {
        return categoriesIds.Select(ci => new Category(string.Empty, ci));
    }
}