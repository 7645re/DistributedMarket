using Carts.Domain.Dto;
using Carts.Domain.Models;

namespace Carts.Domain.Mappers;

public static class CartMapper
{
    public static Cart ToCart(this CartEntity cartEntity)
    {
        return new Cart
        {
            UserId = cartEntity.UserId,
            Items = cartEntity.Items.Select(x => x.ToCartItem()).ToList()
        };
    }

    public static CartItem ToCartItem(this CartItemEntity cartItemEntity)
    {
        return new CartItem
        {
            ProductId = cartItemEntity.ProductId,
            Quantity = cartItemEntity.Quantity
        };
    }

    public static CartEntity ToCartEntity(this CartCreate cart)
    {
        return new CartEntity
        {
            UserId = cart.UserId,
            Items = cart.Items.Select(x => x.ToCartItemEntity()).ToList()
        };
    }

    public static CartItemEntity ToCartItemEntity(this CartItem cartItem)
    {
        return new CartItemEntity
        {
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity
        };
    }
}