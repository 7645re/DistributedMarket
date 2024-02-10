using Catalog.API.Dto.Requests.Category;
using Catalog.Domain.Dto;

namespace Catalog.API.Mappers;

public static class CategoryMapper
{
    public static Category ToCategory(this CategoryUpdate categoryUpdate, int id)
    {
        return new Category { Id = id, Name = categoryUpdate.Name };
    }
    
    public static Category ToCategory(this CategoryCreate categoryCreate)
    {
        return new Category { Name = categoryCreate.Name };
    }
}