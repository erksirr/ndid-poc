using ndid_poc.Application.Services;

namespace ndid_poc.API.Endpoints;

public static class MockEndpoints
{
    public static void MapMockEndpoints(this WebApplication app)
    {
        app.MapPost("/mock/callback/{requestId}", (
            string requestId,
            CallbackService service,
            string status = "confirmed") =>
        {
            service.InjectMockCallback(requestId, status);
            return Results.Ok(new { message = $"Mock callback injected with status={status}" });
        });
    }
}
