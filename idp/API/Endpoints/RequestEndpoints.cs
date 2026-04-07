using ndid_idp.Application.Services;
using ndid_idp.Infrastructure.Repositories;

namespace ndid_idp.API.Endpoints;

public static class RequestEndpoints
{
    public static void MapRequestEndpoints(this WebApplication app)
    {
        // ดู pending requests ทั้งหมด
        app.MapGet("/requests", (IdpRequestRepository repository) =>
            Results.Ok(repository.GetPending()));

        // ดูสถานะ request
        app.MapGet("/requests/{requestId}", (string requestId, IdpRequestRepository repository) =>
        {
            var record = repository.Get(requestId);
            return record is null
                ? Results.NotFound(new { message = "Request ID not found" })
                : Results.Ok(record);
        });

        // ตอบ accept หรือ reject
        app.MapPost("/requests/{requestId}/respond", async (
            string requestId,
            RespondInput input,
            IdpRespondService service) =>
        {
            var result = await service.RespondAsync(requestId, input.Action);
            return result == "not_found"
                ? Results.NotFound(new { message = "Request ID not found" })
                : Results.Ok(new { request_id = requestId, result });
        });
    }
}

record RespondInput(string Action); // accept | reject