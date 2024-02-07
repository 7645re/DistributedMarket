using System.Text.Json.Serialization;
using Catalog.API.Dto.Requests.Category;

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
    public ICollection<int> Categories { get; set; } = ArraySegment<int>.Empty;
}