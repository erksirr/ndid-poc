using System.Text.Json;
using ndid_idp.Infrastructure.Repositories;

namespace ndid_idp.Application.Services;

public class IdpCallbackService(IdpRequestRepository repository, ILogger<IdpCallbackService> logger)
{
    public void HandleIncomingRequest(JsonElement payload)
    {
        var requestId = payload.TryGetProperty("request_id", out var rid) ? rid.GetString() : null;
        logger.LogInformation("IdP received request: requestId={RequestId}", requestId);

        if (requestId != null)
            repository.Save(requestId, payload, "pending");
    }
}