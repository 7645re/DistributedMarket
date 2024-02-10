using Catalog.API.Dto.Category;
using Catalog.Domain.Dto;
using Catalog.Domain.Dto.Category;

namespace Catalog.API.Mappers;

public static class CategoryMapper
{
    public static Category ToCategory(this CategoryUpdateRequest categoryUpdateRequest, int id)
    {
        return new Category { Id = id, Name = categoryUpdateRequest.Name };
    }
    
    public static Category ToCategory(this CategoryCreateRequest categoryCreateRequest)
    {
        return new Category { Name = categoryCreateRequest.Name };
    }
}