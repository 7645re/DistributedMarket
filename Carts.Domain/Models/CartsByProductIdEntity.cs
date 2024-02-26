namespace Carts.Domain.Models;

public class CartsByProductIdEntity
{
    public int ProductId { get; set; }

    public int[] CartIds { get; set; } = Array.Empty<int>();
}