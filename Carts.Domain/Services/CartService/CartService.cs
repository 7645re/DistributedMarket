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
            cancellationToken);

        await UpdateCartsByProductAsync(
            cartEntity.UserId,
            cartCreate.Items.Select(x => x.ProductId).ToList(),
            new List<int>(),
            cancellationToken);
    }

    public async Task UpdateCartAsync(CartUpdate cartUpdate, CancellationToken cancellationToken)
    {
        var existedCart = await _cartRepository.GetByKeyAsync(cartUpdate.UserId.ToString(), cancellationToken);
        if (existedCart is null)
            throw new ArgumentException($"Cart doesnt exist with id: {cartUpdate.UserId}");

        var newProductsIds = cartUpdate.Items.Select(x => x.ProductId).ToList();
        var oldProductsIds = existedCart.Items.Select(x => x.ProductId).ToList();
        var addedProductsIds = newProductsIds.Except(oldProductsIds).ToList();
        var deletedProductsIds = oldProductsIds.Except(newProductsIds).ToList();
        
        await UpdateCartsByProductAsync(
            cartUpdate.UserId,
            addedProductsIds,
            deletedProductsIds,
            cancellationToken);
        
        UpdateCartEntity(existedCart, cartUpdate);
        await _cartRepository.CreateAsync(
            existedCart.UserId.ToString(),
            existedCart,
            cancellationToken);
        
        void UpdateCartEntity(CartEntity cartEntity, CartUpdate cartUpdateLocal)
        {
            cartEntity.Items = cartUpdateLocal.Items.Select(x => x.ToCartItemEntity()).ToList();
        }
    }

    public async Task DeleteDependencyProductAsync(int productId, CancellationToken cancellationToken)
    {
        var cartsByProduct = await _cartsByProductRepository.GetByKeyAsync(
            productId.ToString(),
            cancellationToken);
        if (cartsByProduct is null || !cartsByProduct.CartIds.Any())
            return;

        foreach (var cartId in cartsByProduct.CartIds)
        {
            var cart = await _cartRepository.GetByKeyAsync(cartId.ToString(), cancellationToken);
            if (cart is null)
                continue;
            
            cart.Items.RemoveAll(x => x.ProductId == productId);
            await _cartRepository.CreateAsync(
                cart.UserId.ToString(),
                cart,
                cancellationToken);
        }
        
        await _cartsByProductRepository.RemoveAsync(productId.ToString(), cancellationToken);
    }

    private async Task UpdateCartsByProductAsync(
        int cartId,
        List<int> addedProductsIds,
        List<int> deletedProductsIds,
        CancellationToken cancellationToken)
    {
        foreach (var productId in addedProductsIds)
        {
            var existedCartsByProduct = await _cartsByProductRepository.GetByKeyAsync(
                productId.ToString(),
                cancellationToken);

            if (existedCartsByProduct is null)
            {
                var cartsByProductEntity = new CartsByProductIdEntity
                {
                    ProductId = productId,
                    CartIds = new List<int> { cartId }
                };
                await _cartsByProductRepository.CreateAsync(
                    cartsByProductEntity.ProductId.ToString(),
                    cartsByProductEntity,
                    cancellationToken);
                continue;
            }
            
            existedCartsByProduct.CartIds.Add(cartId);
            await _cartsByProductRepository.CreateAsync(
                existedCartsByProduct.ProductId.ToString(),
                existedCartsByProduct,
                cancellationToken);
        }

        foreach (var productId in deletedProductsIds)
        {
            var existedCartsByProduct = await _cartsByProductRepository.GetByKeyAsync(
                productId.ToString(),
                cancellationToken);
            
            if (existedCartsByProduct is null)
                continue;
            
            existedCartsByProduct.CartIds.Remove(cartId);
            await _cartsByProductRepository.CreateAsync(
                existedCartsByProduct.ProductId.ToString(),
                existedCartsByProduct,
                cancellationToken);
        }
    }
}