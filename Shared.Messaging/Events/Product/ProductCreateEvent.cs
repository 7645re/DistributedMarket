namespace Shared.Messaging.Events.Product;

public class ProductCreateEvent
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Count { get; set; }

    public ICollection<int> Categories { get; set; } = Array.Empty<int>();
    
    public DateTimeOffset Timestamp { get; set; }
}