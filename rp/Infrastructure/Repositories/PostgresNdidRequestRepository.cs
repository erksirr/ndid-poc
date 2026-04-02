using System.Text.Json;
using ndid_poc.Domain.Entities;
using ndid_poc.Infrastructure.Persistence;

namespace ndid_poc.Infrastructure.Repositories;

public class PostgresNdidRequestRepository(AppDbContext db)
{
    public void Save(string requestId, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var existing = db.NdidRequests.Find(requestId);

        if (existing is null)
            db.NdidRequests.Add(new NdidRequest { RequestId = requestId, Payload = json });
        else
            existing.Payload = json;

        db.SaveChanges();
    }

    public bool TryGet(string requestId, out object? data)
    {
        var record = db.NdidRequests.Find(requestId);
        if (record is null) { data = null; return false; }

        data = JsonSerializer.Deserialize<JsonElement>(record.Payload);
        return true;
    }
}
