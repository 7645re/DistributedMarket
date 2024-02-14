using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Category;

public class CategoryUpdateResponse
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}