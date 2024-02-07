using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Requests.Category;

public class CategoryUpdate
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}