using Catalog.API.Dto.Requests.Category;
using Catalog.Domain.Dto;

namespace Catalog.API.Mappers;

public static class CategoryMapper
{
    public static Category ToCategory(this CategoryUpdate categoryUpdate, int id)
    {
        return new Category(categoryUpdate.Name, id);
    }
    
    public static Category ToCategory(this CategoryCreate categoryCreate)
    {
        return new Category(categoryCreate.Name);
    }
}