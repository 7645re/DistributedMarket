namespace Catalog.Messaging.Options;

public class KafkaOptions
{
    public string Address { get; set; } = string.Empty;
    
    public int Port { get; set; }
    
    public string ProductCreateTopic { get; set; } = string.Empty;

    public string ProductUpdateTopic { get; set; } = string.Empty;
    
    public string ProductDeleteTopic { get; set; } = string.Empty;
    
    public string CategoryCreateTopic { get; set; } = string.Empty;
    
    public string CategoryUpdateTopic { get; set; } = string.Empty;
    
    public string CategoryDeleteTopic { get; set; } = string.Empty;
    
    public string GetHost() => $"{Address}:{Port}";
}