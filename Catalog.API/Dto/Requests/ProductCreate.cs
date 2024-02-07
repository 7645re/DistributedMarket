using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Requests;

public class ProductCreate
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("categories")]
    public ICollection<CategoryCreate> Categories { get; set; } = ArraySegment<CategoryCreate>.Empty;
}