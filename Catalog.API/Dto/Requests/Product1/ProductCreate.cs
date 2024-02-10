using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Requests.Product;

public class ProductCreate
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("categories")]
    public ICollection<int> Categories { get; set; } = Array.Empty<int>();
}