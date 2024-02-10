namespace Catalog.Domain.Dto;

public record Category(string Name, int Id = 0)
{
    public readonly string Name = string.Empty;
}
