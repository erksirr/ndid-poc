using System.Text.Json;
using ndid_poc.Infrastructure.Repositories;

namespace ndid_poc.Application.Services;

public class CallbackService(PostgresNdidRequestRepository repository, ILogger<CallbackService> logger)
{
    public void HandleCallback(JsonElement payload)
    {
        var requestId = payload.TryGetProperty("request_id", out var rid) ? rid.GetString() : "unknown";
        var status    = payload.TryGetProperty("status",     out var st)  ? st.GetString()  : "unknown";

        logger.LogInformation("Callback received: requestId={RequestId} status={Status}", requestId, status);

        if (requestId != null)
            repository.Save(requestId, payload);
    }

    public void InjectMockCallback(string requestId, string status)
    {
        var mockPayload = new
        {
            type       = "request_status",
            request_id = requestId,
            status,
            closed     = true,
            timed_out  = false,
            response_list = new[]
            {
                new
                {
                    idp_id          = "idp-mock-01",
                    aal             = "1",
                    ial             = "2.1",
                    status          = "accept",
                    valid_signature = true,
                    valid_ial       = true
                }
            }
        };

        repository.Save(requestId, mockPayload);
        logger.LogInformation("Mock callback injected: requestId={RequestId} status={Status}", requestId, status);
    }
}
