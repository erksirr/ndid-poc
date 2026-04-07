using ndid_as.Application.Services;
using ndid_as.Infrastructure.Repositories;

namespace ndid_as.API.Endpoints;

public static class RequestEndpoints
{
    public static void MapRequestEndpoints(this WebApplication app)
    {
        app.MapGet("/requests", (AsRequestRepository repository) =>
            Results.Ok(repository.GetPending()));

        app.MapGet("/requests/{requestId}", (string requestId, AsRequestRepository repository) =>
        {
            var record = repository.Get(requestId);
            return record is null
                ? Results.NotFound(new { message = "Request ID not found" })
                : Results.Ok(record);
        });

        app.MapPost("/requests/{requestId}/send", async (string requestId, AsDataService service) =>
        {
            var result = await service.SendDataAsync(requestId);
            return result == "not_found"
                ? Results.NotFound(new { message = "Request ID not found" })
                : Results.Ok(new { request_id = requestId, result });
        });
    }
}