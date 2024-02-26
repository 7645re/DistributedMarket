namespace Carts.API.Dto;

public class CartItemRequest
{
    public int ProductId { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
}