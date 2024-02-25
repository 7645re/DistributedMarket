using Catalog.Domain.Dto;
using Catalog.Domain.Dto.Category;
using Catalog.Domain.Models;
using Catalog.Messaging.Events.Category;

namespace Catalog.Domain.Mappers;

public static class CategoryMapper
{
    public static Category ToCategory(this CategoryEntity categoryEntity)
    {
        return new Category { Id = categoryEntity.Id, Name = categoryEntity.Name };
    }

    public static IEnumerable<Category> ToCategories(this IEnumerable<CategoryEntity> categoryEntities)
    {
        return categoryEntities.Select(ce => ce.ToCategory());
    }

    public static CategoryEntity ToCategoryEntity(this CategoryCreate categoryCreate)
    {
        return new CategoryEntity { Name = categoryCreate.Name };
    }

    public static CategoryCreateEvent ToCategoryCreateEvent(this CategoryEntity categoryEntity)
    {
        return new CategoryCreateEvent
        {
            Id = categoryEntity.Id,
            Name = categoryEntity.Name,
            Timestamp = DateTimeOffset.Now
        };
    }

    public static CategoryUpdateEvent ToCategoryUpdateEvent(this CategoryEntity categoryEntity,
        CategoryUpdate categoryUpdate)
    {
        return new CategoryUpdateEvent
        {
            Id = categoryEntity.Id,
            OldName = categoryEntity.Name,
            NewName = categoryUpdate.Name,
            Timestamp = DateTimeOffset.Now
        };
    }
}