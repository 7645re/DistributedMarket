namespace Catalog.Messaging.Events.Category;

public class CategoryDeleteEvent
{
    public int Id { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
}