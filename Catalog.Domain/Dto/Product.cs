namespace Catalog.Domain.Dto;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
    
    public string Description { get; set; } = string.Empty;

    public IEnumerable<Category> Categories { get; set; } = Array.Empty<Category>();
}
