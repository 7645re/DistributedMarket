namespace Carts.Domain.Models;

public class CartsByProductIdEntity
{
    public int ProductId { get; set; }

    public List<int> CartIds { get; set; } = new();
}