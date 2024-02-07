using System.Text.Json.Serialization;

namespace Catalog.API.Dto.Requests.Category;

public class CategoryCreate
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}