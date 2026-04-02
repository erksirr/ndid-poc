using System.Text.Json;
using ndid_poc.Application.Services;

namespace ndid_poc.API.Endpoints;

public static class CallbackEndpoints
{
    public static void MapCallbackEndpoints(this WebApplication app)
    {
        app.MapPost("/ndid/callback", (JsonElement payload, CallbackService service) =>
        {
            service.HandleCallback(payload);
            return Results.Ok();
        });
    }
}
