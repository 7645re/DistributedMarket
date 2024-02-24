using Catalog.API.Dto.Product;
using Catalog.Domain.Dto.Product;
using Catalog.Domain.Models;
using Catalog.Messaging.Events;

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

    public static ProductCreateResponse ToProductCreateResponse(this Product product)
    {
        return new ProductCreateResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Count = product.Count,
            Description = product.Description,
            Categories = product.Categories.Select(c => c.Id).ToArray()
        };
    }

    public static ProductGetResponse ToProductGetResponse(this Product product)
    {
        return new ProductGetResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Count = product.Count,
            Description = product.Description,
            Categories = product.Categories.Select(c => c.Id).ToArray()
        };
    }

    public static ProductUpdateResponse ToProductUpdateResponse(this Product product)
    {
        return new ProductUpdateResponse
        {
            Name = product.Name,
            Price = product.Price,
            Count = product.Count,
            Description = product.Description,
            Categories = product.Categories.Select(c => c.Id).ToArray()
        };
    }
}