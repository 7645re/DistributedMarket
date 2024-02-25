using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Product;

public class ProductUpdateRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("categories")]
    public IList<int>? Categories { get; set; }
}