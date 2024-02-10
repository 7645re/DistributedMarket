using Catalog.API.Dto.Product;
using Catalog.Domain.Dto.Product;

namespace Catalog.API.Mappers;

public static class ProductMapper
{
    public static ProductCreate ToProductCreate(this ProductCreateRequest productCreateRequest)
    {
        return new ProductCreate
        {
            Name = productCreateRequest.Name,
            Price = productCreateRequest.Price,
            Count = productCreateRequest.Count,
            Description = productCreateRequest.Description,
            Categories = productCreateRequest.Categories
        };
    }

    public static ProductUpdate ToProductUpdate(this ProductUpdateRequest productUpdateRequest, int id)
    {
        return new ProductUpdate
        {
            Id = id,
            Name = productUpdateRequest.Name,
            Price = productUpdateRequest.Price,
            Count = productUpdateRequest.Count,
            Description = productUpdateRequest.Description,
            Categories = productUpdateRequest.Categories
        };
    }
}