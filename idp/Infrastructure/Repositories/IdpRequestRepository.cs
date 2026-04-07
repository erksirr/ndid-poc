using System.Text.Json;
using ndid_idp.Domain.Entities;
using ndid_idp.Infrastructure.Persistence;

namespace ndid_idp.Infrastructure.Repositories;

public class IdpRequestRepository(AppDbContext db)
{
    public void Save(string requestId, object data, string status = "pending")
    {
        var json = JsonSerializer.Serialize(data);
        var existing = db.IdpRequests.Find(requestId);

        if (existing is null)
            db.IdpRequests.Add(new IdpRequest { RequestId = requestId, Payload = json, Status = status });
        else
        {
            existing.Payload = json;
            existing.Status  = status;
        }

        db.SaveChanges();
    }

    public IdpRequest? Get(string requestId) => db.IdpRequests.Find(requestId);

    public List<IdpRequest> GetPending() =>
        db.IdpRequests.Where(x => x.Status == "pending").ToList();
}