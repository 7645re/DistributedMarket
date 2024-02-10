using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Category;

public class CategoryUpdateRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}