namespace Catalog.Domain.Dto.Product;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
    
    public string Description { get; set; } = string.Empty;

    public int Count { get; set; }

    public IEnumerable<Category.Category> Categories { get; set; } = Array.Empty<Category.Category>();
}
