using System.Text.Json;
using ndid_as.Infrastructure.Repositories;

namespace ndid_as.Application.Services;

public class AsCallbackService(AsRequestRepository repository, ILogger<AsCallbackService> logger)
{
    public void HandleIncomingRequest(JsonElement payload)
    {
        var requestId = payload.TryGetProperty("request_id", out var rid) ? rid.GetString() : null;
        logger.LogInformation("AS received request: requestId={RequestId}", requestId);

        if (requestId != null)
            repository.Save(requestId, payload, "pending");
    }
}