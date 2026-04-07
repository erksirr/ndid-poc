using System.Text.Json;
using ndid_as.Application.Services;

namespace ndid_as.API.Endpoints;

public static class CallbackEndpoints
{
    public static void MapCallbackEndpoints(this WebApplication app)
    {
        app.MapPost("/ndid/callback", (JsonElement payload, AsCallbackService service) =>
        {
            service.HandleIncomingRequest(payload);
            return Results.Ok();
        });
    }
}