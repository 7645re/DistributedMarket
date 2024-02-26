using Carts.API.Dto;
using Carts.Domain.Dto;

namespace Carts.API.Mappers;

public static class CartMapper
{
    public static CartCreate ToCartCreate(this CartCreateRequest cartCreateRequest)
    {
        return new CartCreate
        {
            UserId = cartCreateRequest.UserId,
            Items = cartCreateRequest.Items.Select(x => x.ToCartItem()).ToList()
        };
    }

    public static CartUpdate ToCartUpdate(this CartUpdateRequest cartUpdateRequest, int userId)
    {
        return new CartUpdate
        {
            UserId = userId,
            Items = cartUpdateRequest.Items.Select(x => x.ToCartItem()).ToList()
        };
    }
    
    public static CartItem ToCartItem(this CartItemRequest cartItemRequest)
    {
        return new CartItem
        {
            ProductId = cartItemRequest.ProductId,
            Quantity = cartItemRequest.Quantity,
            Price = cartItemRequest.Price
        };
    }
}