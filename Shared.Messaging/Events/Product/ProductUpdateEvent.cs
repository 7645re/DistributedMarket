namespace Shared.Messaging.Events.Product;

public class ProductUpdateEvent
{
    public int Id { get; set; }

    public string OldName { get; set; } = string.Empty;
    
    public string? NewName { get; set; }
    
    public decimal OldPrice { get; set; }
    
    public decimal? NewPrice { get; set; }
    
    public int OldCount { get; set; }
    
    public int? NewCount { get; set; }

    public string OldDescription { get; set; } = string.Empty;
    
    public string? NewDescription { get; set; }
    
    public IList<int> OldCategories { get; set; } = Array.Empty<int>();
    
    public IList<int>? NewCategories { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
}