namespace Carts.API.Dto;

public class CartCreateRequest
{
    public int UserId { get; set; }

    public List<CartItemRequest> Items { get; set; } = new();
}