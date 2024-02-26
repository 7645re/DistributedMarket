using Carts.Domain.Dto;
using Carts.Domain.Mappers;
using Carts.Domain.Models;
using Carts.Domain.Repositories.Cart;
using Carts.Domain.Repositories.CartByProduct;

namespace Carts.Domain.Services.CartService;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    
    private readonly ICartsByProductRepository _cartsByProductRepository;

    public CartService(
        ICartRepository cartRepository,
        ICartsByProductRepository cartsByProductRepository)
    {
        _cartRepository = cartRepository;
        _cartsByProductRepository = cartsByProductRepository;
    }

    public async Task<Cart> GetCartAsync(int cartId, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByKeyAsync(cartId.ToString(), cancellationToken);
        if (cart is null)
            throw new ArgumentException($"Cart doesnt exist with id: {cartId}");
        
        return cart.ToCart();
    }

    public async Task CreateCartAsync(CartCreate cartCreate, CancellationToken cancellationToken)
    {
        var existedCart = await _cartRepository.GetByKeyAsync(cartCreate.UserId.ToString(), cancellationToken);
        if (existedCart is not null)
            throw new ArgumentException($"Cart already exist with id: {cartCreate.UserId}");
        
        var cartEntity = cartCreate.ToCartEntity();
        await _cartRepository.CreateAsync(
            cartEntity.UserId.ToString(),
            cartEntity,
            TimeSpan.FromMinutes(5),
            cancellationToken);
        
        await _cartsByProductRepository.CreateAsync(cartEntity.Items, cancellationToken);
    }
    
    public async Task ClearCartAsync(int cartId, CancellationToken cancellationToken)
    {
        var existedCart = await _cartRepository.GetByKeyAsync(cartId.ToString(), cancellationToken);
        if (existedCart is null)
            throw new ArgumentException($"Cart doesnt exist with id: {cartId}");
        
        await _cartRepository.UpdateAsync(cartId.ToString(), new CartEntity
        {
            UserId = existedCart.UserId,
            Items = new List<CartItemEntity>()
        }, cancellationToken);
    }

    public async Task UpdateCartAsync(CartUpdate cartUpdate, CancellationToken cancellationToken)
    {
        var existedCart = await _cartRepository.GetByKeyAsync(cartUpdate.UserId.ToString(), cancellationToken);
        if (existedCart is null)
            throw new ArgumentException($"Cart doesnt exist with id: {cartUpdate.UserId}");
        
        UpdateCartEntity(existedCart, cartUpdate);
        await _cartRepository.UpdateAsync(
            existedCart.UserId.ToString(),
            existedCart,
            cancellationToken);

        void UpdateCartEntity(CartEntity cartEntity, CartUpdate cartUpdateLocal)
        {
            cartEntity.Items = cartUpdateLocal.Items.Select(x => x.ToCartItemEntity()).ToList();
        }
    }

    public async Task DeleteProductFromCart(int productId, CancellationToken cancellationToken)
    {
        
    }
}