using Catalog.API.Dto.Category;
using Catalog.Domain.Dto;
using Catalog.Domain.Dto.Category;

namespace Catalog.API.Mappers;

public static class CategoryMapper
{
    public static CategoryCreate ToCategoryCreate(this CategoryCreateRequest categoryCreateRequest)
    {
        return new CategoryCreate { Name = categoryCreateRequest.Name };
    }

    public static CategoryCreateResponse ToCategoryCreateResponse(this Category category)
    {
        return new CategoryCreateResponse { Id = category.Id, Name = category.Name };
    }

    public static CategoryUpdateResponse ToCategoryUpdateResponse(this Category category)
    {
        return new CategoryUpdateResponse { Name = category.Name };
    }

    public static CategoryUpdate ToCategoryUpdate(this CategoryUpdateRequest categoryUpdateRequest, int id)
    {
        return new CategoryUpdate { Id = id, Name = categoryUpdateRequest.Name };
    }
}