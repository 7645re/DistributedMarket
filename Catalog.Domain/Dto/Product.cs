namespace Catalog.Domain.Dto;

public record Product(string Name, decimal Price, string Description, IEnumerable<Category> Categories, int Id = 0)
{
    public readonly IEnumerable<Category> Categories = ArraySegment<Category>.Empty;
}