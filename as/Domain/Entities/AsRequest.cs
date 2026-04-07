namespace ndid_as.Domain.Entities;

public class AsRequest
{
    public string RequestId  { get; set; } = default!;
    public string Payload    { get; set; } = default!;
    public string Status     { get; set; } = "pending"; // pending | sent | error
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}