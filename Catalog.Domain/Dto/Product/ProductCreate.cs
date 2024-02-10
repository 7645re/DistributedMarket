namespace Catalog.Domain.Dto.Product;

public class ProductCreate
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Count { get; set; }

    public string Description { get; set; } = string.Empty;

    public ICollection<int> Categories { get; set; } = Array.Empty<int>();
}