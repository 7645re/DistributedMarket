namespace Shared.Messaging.Events.Category;

public class CategoryCreateEvent
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
}