namespace Carts.Domain.Dto;

public class CartUpdate
{
    public int UserId { get; set; }

    public List<CartItem> Items { get; set; } = new();
}