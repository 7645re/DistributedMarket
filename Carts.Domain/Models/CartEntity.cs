namespace Carts.Domain.Models;

public class CartEntity
{
    public int UserId { get; set; }
    
    public List<CartItemEntity> Items { get; set; } = new();
}