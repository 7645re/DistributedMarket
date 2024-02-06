using Catalog.Domain.Dto;
using Catalog.Domain.Models;

namespace Catalog.Domain.Mappers;

public static class CategoryMapper
{
    public static Category ToCategory(this CategoryEntity categoryEntity)
    {
        return new Category
        {
            Id = categoryEntity.Id,
            Name = categoryEntity.Name
        };
    }

    public static IEnumerable<Category> ToCategories(this IEnumerable<CategoryEntity> categoryEntities)
    {
        return categoryEntities.Select(ce => ce.ToCategory());
    }
}