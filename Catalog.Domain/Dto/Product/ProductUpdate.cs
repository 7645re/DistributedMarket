namespace Catalog.Domain.Dto.Product;

public class ProductUpdate
{
    public int Id { get; set; }

    public string? Name { get; set; }
    
    public decimal? Price { get; set; }

    public int? Count { get; set; }

    public string? Description { get; set; }

    public IList<int>? Categories { get; set; }
}