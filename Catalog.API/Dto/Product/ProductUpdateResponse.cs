using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Product;

public class ProductUpdateResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("categories")]
    public ICollection<int> Categories { get; set; } = Array.Empty<int>();

}