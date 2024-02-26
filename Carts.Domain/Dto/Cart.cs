namespace Carts.Domain.Dto;

public class Cart
{
    public int UserId { get; set; }

    public List<CartItem> Items { get; set; } = new();
}