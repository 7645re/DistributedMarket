namespace Carts.Domain.Dto;

public class CartItem
{
    public int ProductId { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
}