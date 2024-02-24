namespace Catalog.Messaging.Events;

public class ProductDeleteEvent
{
    public int Id { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
}