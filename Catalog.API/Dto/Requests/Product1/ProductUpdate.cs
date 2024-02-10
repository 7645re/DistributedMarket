using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Requests.Product;

public class ProductUpdate
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("categories")]
    public ICollection<int> Categories { get; set; } = Array.Empty<int>();
}