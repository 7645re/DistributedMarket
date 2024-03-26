namespace Shared.Messaging.Events.Category;

public class CategoryUpdateEvent
{
    public int Id { get; set; }

    public string OldName { get; set; }
    
    public string? NewName { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
}