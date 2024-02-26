namespace Carts.Domain.Options;

public class RedisOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    
    public int CartExpiryInMinutes { get; set; }
    
    public int CartsByProductExpiryInMinutes { get; set; }
}