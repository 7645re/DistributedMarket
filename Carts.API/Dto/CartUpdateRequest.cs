namespace Carts.API.Dto;

public class CartUpdateRequest
{
    public List<CartItemRequest> Items { get; set; } = new();
}