using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Requests;

public class CategoryCreate
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}