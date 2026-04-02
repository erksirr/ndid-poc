namespace ndid_poc.Domain.Entities;

public class NdidRequest
{
    public string RequestId  { get; set; } = default!;
    public string Payload    { get; set; } = default!;  // JSON string
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
