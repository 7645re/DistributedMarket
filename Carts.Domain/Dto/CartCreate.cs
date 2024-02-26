namespace Carts.Domain.Dto;

public class CartCreate
{
    public int UserId { get; set; }

    public List<CartItem> Items { get; set; } = new();
}