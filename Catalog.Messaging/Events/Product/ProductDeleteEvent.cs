namespace Catalog.Messaging.Events.Product;

public class ProductDeleteEvent
{
    public int Id { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
}