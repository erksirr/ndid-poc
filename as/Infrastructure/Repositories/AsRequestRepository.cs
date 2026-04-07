using System.Text.Json;
using ndid_as.Domain.Entities;
using ndid_as.Infrastructure.Persistence;

namespace ndid_as.Infrastructure.Repositories;

public class AsRequestRepository(AppDbContext db)
{
    public void Save(string requestId, object data, string status = "pending")
    {
        var json = JsonSerializer.Serialize(data);
        var existing = db.AsRequests.Find(requestId);

        if (existing is null)
            db.AsRequests.Add(new AsRequest { RequestId = requestId, Payload = json, Status = status });
        else
        {
            existing.Payload = json;
            existing.Status  = status;
        }

        db.SaveChanges();
    }

    public AsRequest? Get(string requestId) => db.AsRequests.Find(requestId);

    public List<AsRequest> GetPending() =>
        db.AsRequests.Where(x => x.Status == "pending").ToList();
}