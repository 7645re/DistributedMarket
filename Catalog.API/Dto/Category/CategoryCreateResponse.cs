using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Category;

public class CategoryCreateResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}