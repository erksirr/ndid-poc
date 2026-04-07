namespace ndid_idp.Domain.Entities;

public class IdpRequest
{
    public string RequestId  { get; set; } = default!;
    public string Payload    { get; set; } = default!;
    public string Status     { get; set; } = "pending"; // pending | accepted | rejected
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}